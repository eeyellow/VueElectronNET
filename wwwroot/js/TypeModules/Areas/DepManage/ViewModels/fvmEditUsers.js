/* ===== 此檔案是自動產生 ===== */
/* ===== 請勿手動變更修改 ===== */

/**
 * 使用者ID
 * @class
 */
class fvmEditUsers {
    /**
     * 流水號
     * @type {number}
     */
    ID;
    /**
     * 使用者ID
     * @type {number}
     */
    UserID;
    /**
     * 部門ID
     * @type {number}
     */
    DepartmentID;
    /**
     * 使用者名稱
     * @type {string}
     */
    Name;
    /**
     * 是否刪除
     * @type {number}
     */
    IsDelete;

    /** 建構式 */
    constructor () {
        this.ID = 0;
        this.ModelError__ID = '';
        this.UserID = 0;
        this.ModelError__UserID = '';
        this.DepartmentID = 0;
        this.ModelError__DepartmentID = '';
        this.Name = "";
        this.ModelError__Name = '';
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
            ['UserID',       'number'],
            ['DepartmentID', 'number'],
            ['Name',         'string'],
            ['IsDelete',     'number'],
        ]);
        if (prop == undefined) {
            return propMap;
        }
        return propMap.get(prop);
    }
}
export {
    fvmEditUsers
}
