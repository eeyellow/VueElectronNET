/**
 * 將字串轉為JSON結構的物件，並賦值
 * @param {any} inputString
 * @param {any} value
 * @param {any} obj*
 * @param {any} keyTransfer
 * @param {any} valueTransfer
 */
export function StringToStruct (inputString, value, obj, keyTransfer, valueTransfer) {
    if (keyTransfer == undefined) {
        keyTransfer = function (key) {
            return `ModelError__${key}`
        }
    }

    if (valueTransfer == undefined) {
        valueTransfer = function (value) {
            return value.join('。')
        }
    }

    const keys = inputString.split('.');
    obj = obj || {};
    let currentLevel = obj;
    keys.forEach((key, index) => {
        const isArray = key.includes('[') && key.includes(']');
        if (isArray) {
            const arrayKey = key.substring(0, key.indexOf('['));
            const arrayIndex = key.substring(
                key.indexOf('[') + 1,
                key.indexOf(']')
            );
            if (!currentLevel[arrayKey]) {
                currentLevel[arrayKey] = [];
            }
            if (!currentLevel[arrayKey][arrayIndex]) {
                currentLevel[arrayKey][arrayIndex] = {};
            }
            if (index === keys.length - 1) {
                if (value !== undefined) {
                    if (valueTransfer !== undefined) {
                        currentLevel[arrayKey][arrayIndex] = valueTransfer(value);
                    }
                    else {
                        currentLevel[arrayKey][arrayIndex] = value;
                    }
                }
            }
            currentLevel = currentLevel[arrayKey][arrayIndex];
        }
        else {
            if (index === keys.length - 1) {
                if (value !== undefined) {
                    if (valueTransfer !== undefined) {
                        if (keyTransfer !== undefined) {
                            currentLevel[keyTransfer(key)] = valueTransfer(value);
                        }
                        else {
                            currentLevel[key] = valueTransfer(value);
                        }
                    }
                    else {
                        if (keyTransfer !== undefined) {
                            currentLevel[keyTransfer(key)] = value;
                        }
                        else {
                            currentLevel[key] = value;
                        }
                    }
                }
            }
            else {
                if (Object.prototype.hasOwnProperty.call(currentLevel, key)) {
                    currentLevel = currentLevel[key]
                }
            }
        }
    });
}

/**
 * 清除結構中的ModelError
 * @param {*} obj
 */
export function CleanStructError (obj) {
    const stack = [obj];
    while (stack?.length > 0) {
        const currentObj = stack.pop();
        Object.keys(currentObj).forEach(key => {
            // console.log(`key: ${key}, value: ${currentObj[key]}`);
            if (typeof currentObj[key] === 'object' && currentObj[key] !== null) {
                stack.push(currentObj[key]);
            }
            if (key.indexOf(`ModelError__`) >= 0) {
                currentObj[key] = ``
            }
        });
    }
}

/**
 * 比對source與target，如果屬性名稱相同，則值也改為相同
 * @param {*} target
 * @param  {...any} sources
 * @returns {any}
 */
export function ModelMapping (target, ...sources) {
    sources.forEach(source => {
        Object.keys(source).forEach(key => {
            const sourceValue = source[key]
            const targetValue = target[key]
            target[key] = targetValue && sourceValue && (typeof targetValue === 'object') && (typeof sourceValue === 'object')
                ? ModelMapping(targetValue, sourceValue)
                : sourceValue
        })
    })
    return target
}

/**
 * 比對source與target，如果屬性名稱相同，則值也改為相同
 * @param {*} target
 * @param  {...any} sources
 * @returns {any}
 */
export function ModelMappingForce (target, ...sources) {
    sources.forEach(source => {
        Object.keys(source).forEach(key => {
            try {
                const sourceValue = source[key]
                const targetValue = target[key]
                const type = Reflect.has(target, 'propertyDictionary') ?
                    target.propertyDictionary(key) :
                    undefined;

                if (targetValue == undefined) {
                    target[key] = TransformMapping(sourceValue, type);
                }
                else {
                    target[key] = sourceValue && (typeof sourceValue === 'object')
                        ? ModelMappingForce(targetValue, sourceValue)
                        : TransformMapping(sourceValue, type)
                }
            }
            catch {
                console.log(source)
                console.log(target)
                console.log(key)
            }
        })
    })
    return target
}

function TransformMapping (sourceValue, type) {
    switch (type) {
        case 'datetime':
            sourceValue = new Intl.DateTimeFormat('Latin', {
                year: 'numeric',
                month: '2-digit',
                day: '2-digit',
                hourCycle: 'h23'
            }).format(new Date(sourceValue));
            break;
        case 'number':
            sourceValue = Number(sourceValue);
            break;
        default:
            break;
    }
    return sourceValue;
}

/**
 * 將JSON物件轉為FormData
 * @param {*} object 
 * @param {*} formData 
 * @param {*} parentKey 
 * @returns 
 */
export function GetFormData(object, formData = new FormData(), parentKey = '') {
    if (object !== null && typeof object === 'object') {
        Object.keys(object).forEach(key => {
            const fullKey = parentKey ? `${parentKey}[${key}]` : key;
            if (object[key] !== null && typeof object[key] === 'object' && !(object[key] instanceof File)) {
                // 遞迴處理巢狀物件
                GetFormData(object[key], formData, fullKey);
            } else {
                // 添加值到 FormData
                if (object[key] !== null) {
                    formData.append(fullKey, object[key]);
                }
            }
        });
    }
    return formData;
}