/* ===== 此檔案是自動產生 ===== */
/* ===== 請勿手動變更修改 ===== */

/**
 * ModelState回應訊息
 * @class
 */
class ModelStateViewModel {
    /**
     * Http狀態碼
     * @type {number}
     */
    StatusCode;
    /**
     * Data
     * @type {any}
     */
    Data;
    /**
     * ModelState
     * @type {KeyValuePair`2[]}
     */
    ModelState;

    /** 建構式 */
    constructor () {
        this.StatusCode = 0;
        this.ModelError__StatusCode = '';
        this.Data = new Object();
        this.ModelError__Data = '';
        this.ModelState = [];
        this.ModelError__ModelState = '';
    }

    /** 
     * 取得屬性的型別
     * @param {string} prop 屬性名稱
     * @returns {string|Map<string, string>} 屬性的型別
     */
    propertyDictionary (prop = undefined) {
        const propMap = new Map([
            ['StatusCode', 'number'],
            ['Data',       'any'],
            ['ModelState', 'KeyValuePair`2[]'],
        ]);
        if (prop == undefined) {
            return propMap;
        }
        return propMap.get(prop);
    }
}
export {
    ModelStateViewModel
}
