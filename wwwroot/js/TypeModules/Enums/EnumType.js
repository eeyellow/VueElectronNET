/* ===== 此檔案是自動產生 ===== */
/* ===== 請勿手動變更修改 ===== */

/**
 * 檔案上傳類型
            注意: 為避免使用者透過改ID即可下載其他檔案
            此處的Enum需分類為
            1.可供訪客下載 
            2.需登入後才可下載
            並於 UploadFileController/Download 進行簡易判斷
 * @enum
 */
const UploadFileRefTypeEnum = Object.freeze({
    Department: Object.freeze({
        Name: `部門管理`,
        Value: 1,
    }),
})

export {
    UploadFileRefTypeEnum
}

/**
 * Vue模式
 * @enum
 */
const VueModeEnum = Object.freeze({
    Edit: Object.freeze({
        Name: `編輯`,
        Value: 1,
    }),
    Detail: Object.freeze({
        Name: `檢視`,
        Value: 2,
    }),
})

export {
    VueModeEnum
}

/**
 * 地支 列舉
 * @enum
 */
const EarthlyBranchesEnum = Object.freeze({
    A: Object.freeze({
        Name: `子`,
        Value: 1,
    }),
    B: Object.freeze({
        Name: `丑`,
        Value: 2,
    }),
    C: Object.freeze({
        Name: `寅`,
        Value: 3,
    }),
    D: Object.freeze({
        Name: `卯`,
        Value: 4,
    }),
    E: Object.freeze({
        Name: `辰`,
        Value: 5,
    }),
    F: Object.freeze({
        Name: `巳`,
        Value: 6,
    }),
    G: Object.freeze({
        Name: `午`,
        Value: 7,
    }),
    H: Object.freeze({
        Name: `未`,
        Value: 8,
    }),
    I: Object.freeze({
        Name: `申`,
        Value: 9,
    }),
    J: Object.freeze({
        Name: `酉`,
        Value: 10,
    }),
    K: Object.freeze({
        Name: `戌`,
        Value: 11,
    }),
    L: Object.freeze({
        Name: `亥`,
        Value: 12,
    }),
})

export {
    EarthlyBranchesEnum
}

/**
 * 天干 列舉
 * @enum
 */
const HeavenlyStemsEnum = Object.freeze({
    A: Object.freeze({
        Name: `甲`,
        Value: 1,
    }),
    B: Object.freeze({
        Name: `乙`,
        Value: 2,
    }),
    C: Object.freeze({
        Name: `丙`,
        Value: 3,
    }),
    D: Object.freeze({
        Name: `丁`,
        Value: 4,
    }),
    E: Object.freeze({
        Name: `戊`,
        Value: 5,
    }),
    F: Object.freeze({
        Name: `己`,
        Value: 6,
    }),
    G: Object.freeze({
        Name: `庚`,
        Value: 7,
    }),
    H: Object.freeze({
        Name: `辛`,
        Value: 8,
    }),
    I: Object.freeze({
        Name: `壬`,
        Value: 9,
    }),
    J: Object.freeze({
        Name: `癸`,
        Value: 10,
    }),
})

export {
    HeavenlyStemsEnum
}

