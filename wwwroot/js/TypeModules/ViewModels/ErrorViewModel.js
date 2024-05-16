/* ===== 此檔案是自動產生 ===== */
/* ===== 請勿手動變更修改 ===== */

/**
 * Represents the view model for displaying error information.
 * @class
 */
class ErrorViewModel {
    /**
     * Gets or sets the request ID.
     * @type {string}
     */
    RequestId;
    /**
     * Gets a value indicating whether the request ID should be shown.
     * @type {boolean}
     */
    ShowRequestId;

    /** 建構式 */
    constructor () {
        this.RequestId = "";
        this.ModelError__RequestId = '';
        this.ShowRequestId = false;
        this.ModelError__ShowRequestId = '';
    }

    /** 
     * 取得屬性的型別
     * @param {string} prop 屬性名稱
     * @returns {string|Map<string, string>} 屬性的型別
     */
    propertyDictionary (prop = undefined) {
        const propMap = new Map([
            ['RequestId',     'string'],
            ['ShowRequestId', 'boolean'],
        ]);
        if (prop == undefined) {
            return propMap;
        }
        return propMap.get(prop);
    }
}
export {
    ErrorViewModel
}
