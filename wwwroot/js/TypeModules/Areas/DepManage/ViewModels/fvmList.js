/* ===== 此檔案是自動產生 ===== */
/* ===== 請勿手動變更修改 ===== */

/**
 * fvmList
 * @class
 */
class fvmList {
    /**
     * @type {number}
     */
    ID;
    /**
     * @type {string}
     */
    Name;
    /**
     * @type {string}
     */
    Alias;
    /**
     * @type {string}
     */
    ParentDepName;
    /**
     * @type {datetime}
     */
    EstablishDate;
    /**
     * @type {string}
     */
    EstablishDateString;

    /** 建構式 */
    constructor () {
        this.ID = 0;
        this.ModelError__ID = '';
        this.Name = "";
        this.ModelError__Name = '';
        this.Alias = "";
        this.ModelError__Alias = '';
        this.ParentDepName = "";
        this.ModelError__ParentDepName = '';
        this.EstablishDate = "";
        this.ModelError__EstablishDate = '';
        this.EstablishDateString = "";
        this.ModelError__EstablishDateString = '';
    }

    /** 
     * 取得屬性的型別
     * @param {string} prop 屬性名稱
     * @returns {string|Map<string, string>} 屬性的型別
     */
    propertyDictionary (prop = undefined) {
        const propMap = new Map([
            ['ID',                  'number'],
            ['Name',                'string'],
            ['Alias',               'string'],
            ['ParentDepName',       'string'],
            ['EstablishDate',       'datetime'],
            ['EstablishDateString', 'string'],
        ]);
        if (prop == undefined) {
            return propMap;
        }
        return propMap.get(prop);
    }
}
export {
    fvmList
}
