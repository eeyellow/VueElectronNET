
(function () {
    window.LC = window.LC || {};
})();


(function () {
    const GetLocalizedString = window.GetLocalizedString || function (msg) { return msg }
    // 全域參數
    const GlobalOptions = {
        // 套用 Search 記憶
        ApplySearchModel: true,
        // 顯示訊息
        AlertMsg: (msg, type, data = null) => {
            //alert(`${type}\n${msg}`)

            swal({
                title: msg,
                type: type,
                confirmButtonText: '確定',
                confirmButtonClass: "btn btn-info mr-2"
            })
        },
        // 刪除確認
        DeleteConfirm: (deleteEvent) => {
            swal({
                title: '刪除提示',
                text: '確定要刪除嗎',
                type: "warning",
                showCancelButton: true,
                confirmButtonText: '確定',
                cancelButtonText: '取消'
            }).then(({ value, dismiss }) => {
                if (value) {
                    deleteEvent()
                }
                else if (dismiss === swal.DismissReason.cancel) {

                }
            })
        },
        // 載入資料前鎖定Grid
        BlockLoading: (searchBlock, tableBlock) => {
            searchBlock.style.pointerEvents = "none"
            searchBlock.style.opacity = "0.8"
            tableBlock.style.pointerEvents = "none"
            tableBlock.style.opacity = "0.8"
        },
        // 載入資料完成解除鎖定Grid
        BlockLoaded: (searchBlock, tableBlock) => {
            searchBlock.style.pointerEvents = "auto"
            searchBlock.style.opacity = "1"
            tableBlock.style.pointerEvents = "auto"
            tableBlock.style.opacity = "1"
        },
        // 載入發生錯誤
        ErrorLoaded: (searchBlock, tableBlock, err) => {
            tableBlock.style.pointerEvents = "none"
            tableBlock.style.opacity = "0.2"
        },
        // 語系字典
        LangDictionary: {
            NoSelectWarning: GetLocalizedString('刪除未選擇項目提示'),
            NotFound: GetLocalizedString('查無資料'),
            PerPage: GetLocalizedString('每頁'),
            Unit: GetLocalizedString('筆'),
            First: GetLocalizedString('第'),
            Page: GetLocalizedString('頁'),
            Total: GetLocalizedString('共'),
            PreviewPage: GetLocalizedString('上一頁'),
            NextPage: GetLocalizedString('下一頁'),
            Goto: GetLocalizedString('跳至'),
        }
    }

    LC.Grid2 = function (element, options) {

        const arrayMap = Array.prototype.map

        // 自訂工具
        const Tool = (() => {

            // 取得GUID
            const createUUID = () => {
                let d = Date.now()
                if (typeof performance !== "undefined" && typeof performance.now === "function") {
                    d += performance.now()
                }
                return "xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx".replace(/[xy]/g, (c) => {
                    let r = (d + cryptoRand() * 16) % 16 | 0
                    d = Math.floor(d / 16)
                    return (c === 'x' ? r : (r & 0x3 | 0x8)).toString(16)
                })
            }

            return {
                NewGuid: () => createUUID()
            }
        })()

        // 預設參數
        const DefaultOptions = {
            /**
             * 初始化
             * @param searchBlock 搜尋區塊
             * @param tableBlock  表格區塊
             * @param grid        顯示資料區塊
             */
            initialization: (searchBlock, tableBlock, grid) => { },

            // 搜尋區塊 Class 名稱
            searchBlockClass: "LCGD2-search",
            // 搜尋區塊 不記憶的 Class名稱
            searchNotMemoryCloumnClass: "LCGD2-search-notMemory",
            // 搜尋區塊 要搜尋的元素類別
            searchElementType: [
                "input[type='text']",
                "input[type='number']",
                "input[type='date']",
                "input[type='datetime-local']",
                "input[type='hidden']",
                "input:checked",
                "select",
            ],
            // 表格區塊 Class 名稱
            tableBlockClass: "LCGD2-grid",
            // 資料區塊 Class 名稱
            dataBlockClass: "LCGD2-data",
            // 分頁器區塊 Class 名稱
            pagerBlockClass: "LCGD2-pager",
            // 搜尋按鈕 Class 名稱
            queryBtnClass: "LCGD2-btn-Query",
            queryAllBtnClass: "LCGD2-btn-QueryAll",
            // 刪除按鈕 Class 名稱
            deleteBtnClass: "LCGD2-btn-Delete",
            // 自定義操作按鈕 Class 名稱
            customBtnClass: "LCGD2-btn-Custom",
            // 全選CheckBox Class 名稱
            selectAllCheckBoxClass: "LCGD2-cb-SelectAll",
            // 標記 點擊後不觸發onClickRow事件
            cullClickRowClass: "LCGD2-cull-cell",
            // 識別ID欄位名稱
            identityField: "",
            // 預設已勾選資料ID清單
            defaultSelectedIDs: [],
            // 預設鎖定資料ID清單
            defaultblockIDs: [],

            /**
             * 啟用選擇CheckBox
             * true:  在呼叫dataViewTemplate時會丟出checkBox，並綁上相關事件
             * false: 無動作
             */
            enableSelect: false,

            /**
             * 資料行檢視範本
             * @param item 單筆資料
             * @param sn 序號
             * @param checkBox 核選方塊(enableSelect為true時才會有值)
             */
            dataViewTemplate: (item, sn, checkBox) => { },

            /**
              * 資料查詢為空時顯示
              */
            emptyTemplate: (dataRowClassName) => {
                return `<div class="row list-body table-nodata ${dataRowClassName}">${GlobalOptions.LangDictionary.NotFound}</div>`
            },

            /**
             * 啟用跨頁選取資料
             * true:  切換分頁時不會取消勾選; 查詢/列出全部，都會取消勾選
             * false: 切換分頁/查詢/列出全部，都會取消勾選
             */
            enableSpreadPageSelect: false,

            /**
             * 啟用選擇器模式
             * true:  切換分頁/查詢/列出全部，都會記憶已勾選資料
             * false: 依enableSpreadPageSelect設定
             */
            enableSelectorMode: false,



            /**
             * 啟用點選資料行
             * true: 滑鼠會改變為pointer，點擊row會觸發onClickRow事件
             * false: 無動作
             */
            enableClickRow: false,

            /**
             * 只能選取一筆資料
             * true:  只可以選取一筆
             * false: 允許多選(預設)
             */
            onlyOneSelect: false,

            /**
             * 勾選資料
             * @param identity 識別ID
             * @param isSelected 是否✔
             * @param item 該筆資料
             */
            onPickRow: (identity, isSelected, item) => { },

            /**
             * 資料繫結前
             * @param grid 顯示資料區塊
             */
            beforeDataBind: (grid) => { return true; },

            /**
             * 資料繫結完成後         
             * @param grid 顯示資料區塊
             * @param data 列表所有資料
             */
            afterDataBind: (grid, data) => { },

            /**
             * 列出全部資料之前callback
             * @param searchBlock 搜尋條件區塊
             */
            beforeQueryAll: (searchBlock) => { },

            /**
             * 載入上次搜尋條件後
             * @param searchModel 搜尋條件
             */
            afterLoadSearchModel: (searchModel) => { },

            /**
             * 點擊資料行
             * @param identity 識別ID
             * @param item 該筆資料
             */
            onClickRow: (identity, item) => { },

            /**
             * 打包完成搜尋條件後
             * @param searchModel 搜尋條件
             */
            afterPackSearchModel: (searchModel) => { },

            /**
             * 初始化後是否馬上搜尋
             * true: 馬上搜尋
             * false: 不搜尋
             */
            immediatelyDatabind: true,

            isApplyGuid: true,
        }

        // Grid 主要功能
        const lcgrid2Module = (() => {

            // 全域參數            
            const globalOptions = Object.assign(GlobalOptions, LC.Grid2GlobalOptions)
            // 設定值
            const settings = Object.assign(DefaultOptions, options)
            // Grid物件
            const gridElement = element

            // 搜尋區塊
            const searchBlock = gridElement.querySelector(`.${settings.searchBlockClass}`)
            // 表格區塊
            const tableBlock = gridElement.querySelector(`.${settings.tableBlockClass}`)
            // 資料區塊
            const dataBlock = gridElement.querySelector(`.${settings.dataBlockClass}`)
            // 分頁區塊
            const pagerBlock = gridElement.querySelector(`.${settings.pagerBlockClass}`)
            // 查詢按鈕
            const queryBtn = gridElement.querySelector(`.${settings.queryBtnClass}`)
            // 列出全部按鈕
            const queryAllBtn = gridElement.querySelector(`.${settings.queryAllBtnClass}`)
            // 刪除按鈕
            const deleteBtn = gridElement.querySelector(`.${settings.deleteBtnClass}`)
            // 自訂義動作按鈕
            const customBtn = gridElement.querySelector(`.${settings.customBtnClass}`)

            // 資料勾選CheckBox Class名稱
            const selectCheckBoxClassName = "LCGD2-checkbox"
            // 資料行Class名稱
            const dataRowClassName = "LCGD2-data-row"

            // 目前清單資料集
            let dataSource = {}
            // 目前清單已選取的ID
            let selectedIDs = settings.defaultSelectedIDs.map(x => x.toString())
            // 目前清單已鎖定的ID
            let blockIDs = settings.defaultblockIDs.map(x => x.toString())
            // 分頁大小, 目前所在頁數, 排序欄位
            let pageSize = 0, nowPage = 1, sortField = [], sortAction = []


            // 取得排序圖示
            const getSortIcon = (sortType) =>
                `<i class="${getIconClass(sortType)}"></i>`
            // 取得排序圖示class
            const getIconClass = (sortType) => {
                switch (sortType.toLowerCase()) {
                    case "asc":
                        return "sortIcon fa fa-caret-up"
                    case "desc":
                        return "sortIcon fa fa-caret-down"
                    default:
                        return "sortIcon fa fa-caret-left"
                }
            }
            // 排序欄位 註冊點擊事件
            const sortFieldClickEvent = elSort => {
                elSort.addEventListener("click", ({ currentTarget }) => {
                    //if (e.target.tagName !== document.createElement("a").tagName) {
                    //    return
                    //}

                    // 取得目前排序欄位/升降冪
                    let field = currentTarget.dataset.sortname
                    let action = currentTarget.dataset.sortaction

                    if (action === undefined || action.toLowerCase() === "desc") {
                        action = "asc"
                    } else {
                        action = "desc"
                    }

                    // 所有欄位排序圖示設定為沒排序
                    let sortIcons = currentTarget.parentNode.getElementsByClassName("sortIcon")
                    arrayMap.call(sortIcons, icon => icon.className = getIconClass(""))
                    // 切換目前欄位排序圖示                        
                    currentTarget.querySelector(".sortIcon").remove()
                    currentTarget.innerHTML += getSortIcon(action)
                    currentTarget.dataset.sortaction = action

                    sortField = [field]
                    sortAction = [action]
                    dataBind()
                })
            }

            // 初始化
            const __init = (() => {
                // 檢查各參數
                if (!gridElement.dataset.datasource || gridElement.dataset.datasource === null || gridElement.dataset.datasource === "") {
                    throw "請設定DataSource Url"
                }
                if (settings.enableSelect && settings.identityField === "") {
                    throw "啟用CheckBox時，identityField必須指定欄位名稱"
                }
                if (settings.enableSpreadPageSelect && !settings.enableSelect) {
                    throw "啟用跨頁選取資料時，必須啟用enableSelect"
                }
                if (settings.enableSelectorMode && !settings.enableSelect) {
                    throw "啟用選擇器模式時，必須啟用enableSelect"
                }
                if (settings.deleteBtn != null) {
                    if (!gridElement.dataset.deltarget || gridElement.dataset.deltarget === null || gridElement.dataset.deltarget === "") {
                        throw "請設定deltarget Url"
                    }
                }
                if (settings.customBtn != null) {
                    if (!gridElement.dataset.customtarget || gridElement.dataset.customtarget === null || gridElement.dataset.customtarget === "") {
                        throw "請設定customtarget Url"
                    }
                }

                // 初始化
                settings.initialization(searchBlock, tableBlock, dataBlock)
                // 載入設定
                settings.queryUrl = gridElement.dataset.datasource
                settings.deleteUrl = gridElement.dataset.deltarget
                settings.customUrl = gridElement.dataset.customtarget
                settings.guid = gridElement.dataset.guid
                // 載入列表變數
                pageSize = parseInt(gridElement.dataset.pagesize || 50)
                nowPage = parseInt(gridElement.dataset.nowpage || 1)
                sortField = (gridElement.dataset.sortfield || "").split(",")
                sortAction = (gridElement.dataset.sortaction || "").split(",")
                // 自定義Swal
                swalTitle = gridElement.dataset.swaltitle || "Title";
                swalText = gridElement.dataset.swaltext || "Text";

                // 送出查詢
                const searchQuery = () => {
                    // 只要不是[選擇器模式]，都不記已勾選的資料
                    if (!settings.enableSelectorMode) {
                        selectedIDs = []
                    }
                    nowPage = 1
                    dataBind()
                }
                // 清空搜尋條件
                const clearSearch = () => {
                    searchBlock.querySelectorAll("select:not(.exclude)").forEach((item) => {
                        if (item.parentNode.classList.contains('bootstrap-select')) {
                            $(item).selectpicker("val", "")
                        }
                        else if (item.classList.contains("selectpicker")) {
                            $(item).selectpicker("val", "")
                        }
                        else {
                            //先找有沒有option有設定為default
                            let defaultOption = Array.from(item.querySelectorAll('option')).find(o => o.hasAttribute('default'));
                            if (defaultOption) {
                                item.value = defaultOption.value;
                            }
                            else {
                                //找出全部的option值
                                let optionVals = Array.from(item.querySelectorAll('option')).map(o => o.value)

                                if (optionVals.includes("")) { //如果有""，就設為""
                                    item.value = ""
                                }
                                else { //如果沒有""，就設為最小值(通常會是0)
                                    item.value = Math.min(...Array.from(item.querySelectorAll('option')).map(o => o.value))
                                }
                            }
                        }
                    })
                    searchBlock.querySelectorAll("input:not(.exclude):not([type='radio']):not([type='checkbox'])").forEach((item) => {
                        item.value = ""
                    })
                    searchBlock.querySelectorAll("input[type='checkbox']:not(.exclude)").forEach((item) => {
                        item.removeAttribute("checked")
                        item.checked = false
                    })
                }

                // 建立排序欄位
                const createSortField = () => {
                    // 初始化有排序功能的欄位
                    tableBlock.querySelectorAll("[data-sortname]").forEach((elSort) => {
                        // 預設排序圖示
                        let sortIcon = getSortIcon("")

                        let fieldName = elSort.dataset.sortname
                        let sortIndex = sortField.indexOf(fieldName)

                        if (sortIndex !== -1) {
                            sortIcon = getSortIcon(sortAction[sortIndex])
                            elSort.dataset.sortaction = sortAction[sortIndex]
                        }
                        elSort.innerHTML += sortIcon

                        // 排序事件
                        sortFieldClickEvent(elSort)
                    })
                }

                // 刪除已選擇資料
                const deleteSelected = () => {

                    const postData = new URLSearchParams()
                    selectedIDs.map(p => {
                        postData.append("deleteIDs", p)
                    })

                    fetch(settings.deleteUrl, {
                        method: "POST",
                        headers: {
                            "RequestVerificationToken": document.getElementsByName("__RequestVerificationToken")[0].value,
                            "accept": "application/json",
                            "content-type": "application/x-www-form-urlencoded",
                            "credentials": "include",
                            "X-Requested-With": "XMLHttpRequest",
                        },
                        body: postData
                    }).then((response) => {
                        if (response.status === 403 || response.status === 401) {
                            const error = new Error()
                            error.status = response.status
                            throw error
                        }
                        return response.json()
                    }).then((data) => {
                        if (data.Status === LayoutPageScope.SuccessEnum.Success) {
                            globalOptions.AlertMsg(data.Message, "success")
                            selectedIDs = []
                            dataBind()
                        } else {
                            globalOptions.AlertMsg(data.Message, "error", data)
                        }
                    }).catch((err) => {
                        if (err.status === 403) {
                            globalOptions.AlertMsg('無權限', "error", null)
                        } else if (err.status === 401) {
                            let promise = globalOptions.AlertMsg('未登入', "error", null)
                            if (promise) {
                                promise.then(() => {
                                    location.href = "/Home/Login"
                                })
                            }
                        }
                        else {
                            throw err
                        }
                    })
                }

                // 送出自定義操作
                const customSelected = () => {
                    const postData = new URLSearchParams()
                    selectedIDs.map(p => {
                        postData.append("selectIDs", p)
                    })

                    fetch(settings.customUrl, {
                        method: "POST",
                        headers: {
                            "RequestVerificationToken": document.getElementsByName("__RequestVerificationToken")[0].value,
                            "accept": "application/json",
                            "content-type": "application/x-www-form-urlencoded",
                            "credentials": "include",
                            "X-Requested-With": "XMLHttpRequest",
                        },
                        body: postData
                    }).then((response) => {
                        if (response.status === 403 || response.status === 401) {
                            const error = new Error()
                            error.status = response.status
                            throw error
                        }
                        return response.json()
                    }).then((data) => {
                        if (data.Status === LayoutPageScope.SuccessEnum.Success) {
                            globalOptions.AlertMsg(data.Msg, "success")
                            selectedIDs = []
                            dataBind()
                        } else {
                            globalOptions.AlertMsg(data.Msg, "error", data)
                        }
                    }).catch((err) => {
                        if (err.status === 403) {
                            globalOptions.AlertMsg('無權限', "error", null)
                        } else if (err.status === 401) {
                            let promise = globalOptions.AlertMsg('未登入', "error", null)
                            if (promise) {
                                promise.then(() => {
                                    location.href = "/Home/Login"
                                })
                            }
                        }
                        else {
                            throw err
                        }
                    })
                }

                // 搜尋事件
                queryBtn.addEventListener("click", (e) => {
                    e.preventDefault();
                    searchQuery()
                })
                searchBlock.addEventListener("keypress", (e) => {
                    if (e.keyCode === 13) {
                        e.preventDefault();
                        searchQuery()
                        return false
                    }
                })

                // 列出全部事件
                queryAllBtn.addEventListener("click", (e) => {
                    e.preventDefault();
                    clearSearch()
                    settings.beforeQueryAll(searchBlock)
                    searchQuery()
                })
                // 全選事件
                gridElement.addEventListener("change", (e) => {
                    if (e.target && e.target.className.indexOf(`${settings.selectAllCheckBoxClass}`) !== -1) {
                        dataBlock.querySelectorAll(`.${selectCheckBoxClassName}`).forEach((elCheckBox) => {
                            // 已鎖定的資料不可再被勾選
                            if (blockIDs.indexOf(elCheckBox.value) === -1) {
                                elCheckBox.checked = e.target.checked
                                // 觸發change事件
                                let evt = document.createEvent("HTMLEvents")
                                evt.initEvent("change", false, true)
                                elCheckBox.dataset.fromall = "true"
                                elCheckBox.dispatchEvent(evt)
                            }
                        })
                    }
                })

                // 刪除事件
                if (deleteBtn !== null) {
                    deleteBtn.addEventListener("click", (e) => {
                        if (selectedIDs.length === 0) {
                            globalOptions.AlertMsg(globalOptions.LangDictionary.NoSelectWarning, "warning")
                            return
                        }
                        globalOptions.DeleteConfirm(deleteSelected)
                    })
                }

                // 自定義事件
                if (customBtn !== null) {
                    customBtn.addEventListener("click", (e) => {
                        if (selectedIDs.length === 0) {
                            globalOptions.AlertMsg(globalOptions.LangDictionary.NoSelectWarning, "warning")
                            return
                        }

                        swal({
                            title: swalTitle,
                            text: swalText,
                            type: "warning",
                            showCancelButton: true,
                            confirmButtonText: '確定',
                            cancelButtonText: '取消'
                        }).then(({ value, dismiss }) => {
                            if (value) {
                                customSelected()
                            }
                            else if (dismiss === swal.DismissReason.cancel) {

                            }
                        })
                    })
                }

                // 載入搜尋條件                
                let searchModel = []
                if (settings.guid && settings.isApplyGuid) {

                    if (!globalOptions.ApplySearchModel) {
                        sessionStorage[settings.guid] = ""
                    }

                    if (sessionStorage[settings.guid]) {
                        searchModel = JSON.parse(sessionStorage[settings.guid])
                        // 載入搜尋條件
                        let fields = searchBlock.querySelectorAll("input[type='text'], input[type='number'], input[type='hidden'], input:checked, select")

                        for (i = 0; i < fields.length; i++) {
                            let element = fields[i]

                            if (element.classList.contains(settings.searchNotMemoryCloumnClass)) {
                                continue
                            }

                            if (element.classList.contains("selectpicker")) {
                                $(element).selectpicker("val", searchModel[element.name])
                            }
                            else if (element.classList.contains("combo-select")) {
                                $(element).append(`
                                    <option value="${searchModel[element.name]}" selected="selected"></option>
                                `)
                            }
                            else {
                                element.value = searchModel[element.name]
                            }


                        }

                        // 載入分頁/排序
                        pageSize = searchModel.PageSize
                        nowPage = searchModel.NowPage
                        sortField = searchModel.SortField.split(",")
                        sortAction = searchModel.SortAction.split(",")
                    }
                }
                // callback-載入搜尋條件之後
                settings.afterLoadSearchModel(searchModel)

                // 建立排序欄位
                createSortField()
            })();

            // 取得搜尋條件
            const getSearchData = () => {
                let res = {}

                let fields = searchBlock
                    .querySelectorAll(DefaultOptions.searchElementType.join(','))

                for (i = 0; i < fields.length; i++) {
                    let el = fields[i]
                    if (typeof el.selectedOptions !== "undefined") {
                        res[el.name] = []
                        for (oIndex = 0; oIndex < el.selectedOptions.length; oIndex++) {
                            res[el.name].push(el.selectedOptions[oIndex].value)
                        }
                    } else {
                        res[el.name] = el.value
                    }
                }
                return res
            }

            // 全選勾選是否取消事件
            const judgeAllCBChecked = () => {
                let cbNodeList = dataBlock.querySelectorAll(`.${selectCheckBoxClassName}`)
                let cbNodeListArray = Array.from(cbNodeList)
                let checkAllCB = gridElement.querySelector(`.${settings.selectAllCheckBoxClass}`)
                let isAllChecked = false

                if (cbNodeListArray.length > 0) {
                    isAllChecked = Array.from(cbNodeList).every(({ checked }) => checked)
                }

                if (checkAllCB !== null) {
                    checkAllCB.checked = isAllChecked
                }
            }

            // 資料繫結
            const dataBind = () => {

                // 取得搜尋條件
                let searchData = getSearchData()

                // searchModel
                searchData.NowPage = nowPage
                searchData.PageSize = pageSize
                searchData.SortField = sortField.join(',')
                searchData.SortAction = sortAction.join(',')

                // 只要不是[選擇器模式]及[啟用跨頁選取]都不記已勾選資料
                if (!settings.enableSelectorMode && !settings.enableSpreadPageSelect) {
                    selectedIDs = []
                }

                // callback-打包完成搜尋條件後
                settings.afterPackSearchModel(searchData)

                // 建立核選方塊
                const createCheckBox = (id, item) => {
                    let checkBox = undefined
                    const name = Tool.NewGuid()
                    // 是否有啟用核選方塊
                    if (settings.enableSelect) {
                        checkBox = `
                            <div class="custom-control custom-checkbox">
                                <input type="checkbox" class="custom-control-input ${selectCheckBoxClassName}" id="${name}" value="${id}">
                                <label class="custom-control-label" for="${name}"></label>
                            </div>`
                    }
                    // 回傳核選方塊
                    return checkBox
                }

                // 初始化核選方塊 / 鎖定資料
                const initCheckBoxAndBlock = () => {

                    dataBlock.querySelectorAll(`.${selectCheckBoxClassName}`).forEach((elCheckBox) => {
                        // 依暫存的已選取清單 勾選/取消
                        elCheckBox.checked = selectedIDs.indexOf(elCheckBox.value) !== -1
                        // 選取事件
                        // 呼叫相對應的callback，暫存勾選ID
                        elCheckBox.addEventListener("change", (el) => {
                            let id = el.target.value
                            let item = dataSource[id]
                            if (el.target.checked) {
                                if (selectedIDs.indexOf(id) === -1) {
                                    selectedIDs.push(id)
                                }
                                if (settings.onlyOneSelect) {
                                    $(`.${selectCheckBoxClassName}`).prop("checked", false)
                                    el.target.checked = true;
                                    selectedIDs = [id]
                                }
                            }
                            else {
                                if (selectedIDs.indexOf(id) !== -1) {
                                    selectedIDs.splice(selectedIDs.indexOf(id), 1)
                                }
                            }

                            settings.onPickRow(id, el.target.checked, item)

                            if (el.target.dataset.fromall === "true") {
                                elCheckBox.dataset.fromall = "false"
                            } else {
                                judgeAllCBChecked()
                            }

                        })
                    })

                    judgeAllCBChecked()
                }

                // 暫存搜尋狀態
                if (settings.guid) {
                    sessionStorage[settings.guid] = JSON.stringify(searchData)
                }

                // callback-資料繫結之前
                if (!settings.beforeDataBind(dataBlock)) {

                    return null;
                }

                // Lock tableBlock
                globalOptions.BlockLoading(searchBlock, tableBlock)

                // 轉換送出data model
                let postData = new URLSearchParams()
                for (let searchDataKey in searchData) {
                    let sData = searchData[searchDataKey]
                    if (Array.isArray(sData)) {
                        for (let sDataKey in sData) {
                            postData.append(searchDataKey, sData[sDataKey])
                        }
                    } else {
                        postData.append(searchDataKey, sData)
                    }
                }

                // 選擇器模式時，需送出目前已選擇資料
                // 後端可做為已選擇資料置頂
                if (settings.enableSelectorMode) {
                    selectedIDs.forEach(p => {
                        postData.append("selectIDs", p)
                    })
                }

                postData.append("__RequestVerificationToken", document.getElementsByName("__RequestVerificationToken")[0].value);
                // 送出request
                fetch(settings.queryUrl, {
                    method: "POST",
                    headers: {
                        "accept": 'application/json',
                        "content-type": "application/x-www-form-urlencoded",
                        "credentials": "include",
                        "X-Requested-With": "XMLHttpRequest",
                    },
                    body: postData
                }).then(response => response.json())
                    .then(data => {
                        dataSource = {}

                        clearOldData();

                        // 顯示資料
                        data.rows.forEach((e, i) => {
                            let sn = i + 1
                            let id = -1

                            // 沒有設定識別欄位，就用sn做為識別ID                            
                            if (e[settings.identityField]) {
                                id = e[settings.identityField]
                            } else {
                                id = sn
                            }
                            // 紀錄目前清單資料集
                            dataSource[id] = e
                            // 建立核選方塊
                            let checkBox = createCheckBox(id, e)
                            // 顯示資料列
                            dataBlock.innerHTML += settings.dataViewTemplate(e, sn, checkBox)

                            // 取得目前資料列
                            let dataNode = dataBlock.lastElementChild

                            // 設定相關資訊
                            dataNode.dataset.id = id
                            dataNode.classList.add(dataRowClassName)
                            // 鎖定資料列
                            if (blockIDs.indexOf(id.toString()) > -1) {
                                dataNode.style.pointerEvents = "none"
                                dataNode.style.opacity = "0.4"
                            }
                            // 加入資料行可點擊樣式
                            if (settings.enableClickRow) {
                                dataNode.style.cursor = "pointer"
                                // 去除不可點擊的樣式
                                let cullClickElements = dataNode.getElementsByClassName(settings.cullClickRowClass)
                                for (let i = 0; i < cullClickElements.length; i++) {
                                    cullClickElements[i].style.cursor = "default"
                                }
                            }
                        })

                        //若data無資料則顯示"查無資料"
                        if (data.rows.length === 0) {
                            dataBlock.innerHTML += settings.emptyTemplate(dataRowClassName)
                        }

                        // 綁定點擊事件
                        if (settings.enableClickRow) {
                            let dataRows = dataBlock.getElementsByClassName(dataRowClassName);
                            for (let i = 0; i < dataRows.length; i++) {
                                dataRows[i].addEventListener("click", (e) => {
                                    let selection = window.getSelection()
                                    // 忽略選取模式
                                    if (selection.toString().length === 0) {
                                        // 只要父層中有包含剔除Class就不觸發事件
                                        const isCull = e.path.some(p => p.classList && p.classList.contains(settings.cullClickRowClass))
                                        if (!isCull) {
                                            let id = e.currentTarget.dataset.id
                                            let item = dataSource[id]
                                            settings.onClickRow(id, item)
                                        }
                                    }
                                })
                            }
                        }

                        // 重新綁定欄位排序事件
                        tableBlock.querySelectorAll("[data-sortname]").forEach(elSort => sortFieldClickEvent(elSort))
                        // 初始核選方塊 / 鎖定資料
                        initCheckBoxAndBlock()
                        // callback-資料繫結之後
                        settings.afterDataBind(dataBlock, dataSource)

                        // 建立分頁器
                        createPager(data.total)
                    })
                    .then(() => {
                        // UnLock tableBlock
                        globalOptions.BlockLoaded(searchBlock, tableBlock)
                    })
                    .catch(err => {
                        globalOptions.BlockLoaded(searchBlock, tableBlock)
                        globalOptions.ErrorLoaded(searchBlock, tableBlock, err)
                        throw err
                    })
            }

            // 清除舊資料
            const clearOldData = () => {
                let oldRowArray = dataBlock.getElementsByClassName(dataRowClassName);
                for (var i = oldRowArray.length - 1; i >= 0; i--) {
                    oldRowArray[0].parentNode.removeChild(oldRowArray[0]);
                }
            }


            // 建立分頁器
            const createPager = (totalItemCount) => {
                // 計算總頁數
                let totalPage = Math.ceil(totalItemCount / pageSize) || 1

                // 產生分頁器所有物件
                pagerBlock.innerHTML = `
                    <div class="form-inline text-center">
                        <div class='mx-auto'>
                            ${globalOptions.LangDictionary.PerPage}&nbsp;
                            <select class="pagesize form-select form-select-sm w-auto d-inline">
                                <option value="10">10</option>
                                <option value="20">20</option>
                                <option value="50">50</option>
                                <option value="100">100</option>
                                <option value="200">200</option>
                            </select>
                            &nbsp;${globalOptions.LangDictionary.Unit}&nbsp;，
                            ${globalOptions.LangDictionary.First}&nbsp;${(nowPage > totalPage ? totalPage : nowPage)}/${totalPage}&nbsp;${globalOptions.LangDictionary.Page}，${globalOptions.LangDictionary.Total}&nbsp;${totalItemCount}&nbsp;${globalOptions.LangDictionary.Unit}，
                            <a class="pageup link-color" href="javascript: void(0)" title="${globalOptions.LangDictionary.PreviewPage}">${globalOptions.LangDictionary.PreviewPage}</a>
                            ︱${globalOptions.LangDictionary.Goto}&nbsp;
                            <select class="changepage form-select form-select-sm w-auto d-inline">
                                ${Array.from({ length: totalPage }, (v, i) => `<option value="${i + 1}">${i + 1}</option>`)}
                            </select>
                            &nbsp;${globalOptions.LangDictionary.Page}︱
                            <a class="pagedown link-color" href="javascript: void(0)" title="${globalOptions.LangDictionary.NextPage}">${globalOptions.LangDictionary.NextPage}</a>
                        </div>
                    </div>`

                // 分頁大小切換選單
                let pageSizeSelect = pagerBlock.querySelector(".pagesize")
                // 跳頁選單
                const jumpPageSelect = pagerBlock.querySelector(".changepage")
                // 上一頁按鈕
                const previousBtn = pagerBlock.querySelector(".pageup")
                // 下一頁按鈕
                const nextBtn = pagerBlock.querySelector(".pagedown")

                // 初始化分頁功能
                pageSizeSelect.value = pageSize
                pageSizeSelect.addEventListener("change", (e) => {
                    pageSize = e.target.value
                    nowPage = 1
                    dataBind()
                })
                jumpPageSelect.value = nowPage
                jumpPageSelect.addEventListener("change", (e) => {
                    nowPage = e.target.value
                    dataBind()
                })
                if (nowPage > 1) {
                    previousBtn.addEventListener("click", () => {
                        nowPage--
                        dataBind()
                    })
                }
                if (nowPage < totalPage) {
                    nextBtn.addEventListener("click", () => {
                        nowPage++
                        dataBind()
                    })
                }
            }

            return {
                reload: () => dataBind(),
            }

        })()

        if (DefaultOptions.immediatelyDatabind) {
            lcgrid2Module.reload()
        }

        return lcgrid2Module
    }
    
    function cryptoRand () {
        const randomBuffer = new Uint32Array(1);
        (window.crypto || window.msCrypto).getRandomValues(randomBuffer);
        return (randomBuffer[0] / (0xffffffff + 1));
    }
})();
