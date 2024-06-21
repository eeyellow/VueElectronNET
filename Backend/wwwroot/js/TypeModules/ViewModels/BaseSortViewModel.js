/* ===== 此檔案是自動產生 ===== */
/* ===== 請勿手動變更修改 ===== */

/**
 * 基底排序模型
 * @class
 */
class BaseSortViewModel {
    /**
     * 目前頁數(default:1 不可為負)
     * @type {number}
     */
    NowPage;
    /**
     * 分頁筆數(default:10 不可為負)
     * @type {number}
     */
    PageSize;
    /**
     * 目前排序欄位
     * @type {string}
     */
    SortField;
    /**
     * 排序方法
     * @type {string}
     */
    SortAction;
    /**
     * 打開進階查詢
     * @type {boolean}
     */
    OpenSearchMore;

    /** 建構式 */
    constructor () {
        this.NowPage = 0;
        this.ModelError__NowPage = '';
        this.PageSize = 0;
        this.ModelError__PageSize = '';
        this.SortField = "";
        this.ModelError__SortField = '';
        this.SortAction = "";
        this.ModelError__SortAction = '';
        this.OpenSearchMore = false;
        this.ModelError__OpenSearchMore = '';
    }

    /** 
     * 取得屬性的型別
     * @param {string} prop 屬性名稱
     * @returns {string|Map<string, string>} 屬性的型別
     */
    propertyDictionary (prop = undefined) {
        const propMap = new Map([
            ['NowPage',        'number'],
            ['PageSize',       'number'],
            ['SortField',      'string'],
            ['SortAction',     'string'],
            ['OpenSearchMore', 'boolean'],
        ]);
        if (prop == undefined) {
            return propMap;
        }
        return propMap.get(prop);
    }
}
export {
    BaseSortViewModel
}
