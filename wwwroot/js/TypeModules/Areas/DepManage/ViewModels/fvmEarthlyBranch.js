/* ===== 此檔案是自動產生 ===== */
/* ===== 請勿手動變更修改 ===== */

/**
 * 地支ID
 * @class
 */
class fvmEarthlyBranch {
    /**
     * 流水號
     * @type {number}
     */
    ID;
    /**
     * 地支ID
     * @type {number}
     */
    EnumValue;
    /**
     * 部門ID
     * @type {number}
     */
    DepartmentID;
    /**
     * 是否刪除
     * @type {number}
     */
    IsDelete;

    /** 建構式 */
    constructor () {
        this.ID = 0;
        this.ModelError__ID = '';
        this.EnumValue = 0;
        this.ModelError__EnumValue = '';
        this.DepartmentID = 0;
        this.ModelError__DepartmentID = '';
        this.IsDelete = 0;
        this.ModelError__IsDelete = '';
    }

    /** 
     * 取得屬性的型別
     * @param {string} prop 屬性名稱
     * @returns {string|Map<string, string>} 屬性的型別
     */
    propertyDictionary (prop = undefined) {
        const propMap = new Map([
            ['ID',           'number'],
            ['EnumValue',    'number'],
            ['DepartmentID', 'number'],
            ['IsDelete',     'number'],
        ]);
        if (prop == undefined) {
            return propMap;
        }
        return propMap.get(prop);
    }
}
export {
    fvmEarthlyBranch
}
