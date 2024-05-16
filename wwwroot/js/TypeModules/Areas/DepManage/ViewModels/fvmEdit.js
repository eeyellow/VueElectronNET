/* ===== 此檔案是自動產生 ===== */
/* ===== 請勿手動變更修改 ===== */

/**
 * 新增/編輯的ViewModel
 * @class
 */
class fvmEdit {
    /**
     * 流水號
     * @type {number}
     */
    ID;
    /**
     * Vue模式
     * @type {number}
     */
    VueMode;
    /**
     * 名稱
     * @type {string}
     */
    Name;
    /**
     * 縮寫
     * @type {string}
     */
    Alias;
    /**
     * 上層部門的ID
     * @type {number}
     */
    ParentID;
    /**
     * 成立日期
     * @type {datetime}
     */
    EstablishDate;

    /** 建構式 */
    constructor () {
        this.ID = 0;
        this.ModelError__ID = '';
        this.VueMode = 0;
        this.ModelError__VueMode = '';
        this.Name = "";
        this.ModelError__Name = '';
        this.Alias = "";
        this.ModelError__Alias = '';
        this.ParentID = 0;
        this.ModelError__ParentID = '';
        this.EstablishDate = "";
        this.ModelError__EstablishDate = '';
    }

    /** 
     * 取得屬性的型別
     * @param {string} prop 屬性名稱
     * @returns {string|Map<string, string>} 屬性的型別
     */
    propertyDictionary (prop = undefined) {
        const propMap = new Map([
            ['ID',            'number'],
            ['VueMode',       'number'],
            ['Name',          'string'],
            ['Alias',         'string'],
            ['ParentID',      'number'],
            ['EstablishDate', 'datetime'],
        ]);
        if (prop == undefined) {
            return propMap;
        }
        return propMap.get(prop);
    }
}
export {
    fvmEdit
}
