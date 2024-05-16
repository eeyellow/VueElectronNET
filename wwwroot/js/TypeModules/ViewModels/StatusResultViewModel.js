/* ===== 此檔案是自動產生 ===== */
/* ===== 請勿手動變更修改 ===== */

/**
 * Http回應狀態碼與訊息
 * @class
 */
class StatusResultViewModel {
    /**
     * Http狀態碼
     * @type {number}
     */
    StatusCode;
    /**
     * 回應訊息
     * @type {string[]}
     */
    Message;
    /**
     * 回應資料
     * @type {any}
     */
    Data;

    /** 建構式 */
    constructor () {
        this.StatusCode = 0;
        this.ModelError__StatusCode = '';
        this.Message = [];
        this.ModelError__Message = '';
        this.Data = new Object();
        this.ModelError__Data = '';
    }

    /** 
     * 取得屬性的型別
     * @param {string} prop 屬性名稱
     * @returns {string|Map<string, string>} 屬性的型別
     */
    propertyDictionary (prop = undefined) {
        const propMap = new Map([
            ['StatusCode', 'number'],
            ['Message',    'string[]'],
            ['Data',       'any'],
        ]);
        if (prop == undefined) {
            return propMap;
        }
        return propMap.get(prop);
    }
}
export {
    StatusResultViewModel
}
