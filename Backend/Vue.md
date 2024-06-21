# Vue開發說明

前端套件都是安裝在www/node_modules下，統一使用npm管理。

## 安裝套件

### 1. [Vue](https://vuejs.org/guide/quick-start.html)

安裝

```sh
npm install vue
```

```html
<script src="~/node_modules/vue/dist/vue.global.js" asp-append-version="true"></script>
```

### 2. 列表套件: [vue-lcgrid](https://www.npmjs.com/package/@linkchain/vue-lcgrid)

安裝

```sh
npm install @linkchain/vue-lcgrid@0.0.13
```

```html
<link rel="stylesheet" href="~/node_modules/@@linkchain/vue-lcgrid/dist/lcGridVue.css" asp-append-version="true" />
<script src="~/node_modules/@@linkchain/vue-lcgrid/dist/lcGridVue.js" asp-append-version="true"></script>
```

### 3. 日期時間套件: [vue-datepicker](https://vue3datepicker.com/)

安裝

```sh
npm install @vuepic/vue-datepicker
```

```html
<link rel="stylesheet" href="~/node_modules/@@vuepic/vue-datepicker/dist/main.css" asp-append-version="true" />
<script src="~/node_modules/@@vuepic/vue-datepicker/dist/vue-datepicker.iife.js" asp-append-version="true"></script>
```

### 4. 下拉選單套件: [vue-select](https://vue-select.org/)

安裝

```sh
npm install vue-select@beta
```

```html
<link rel="stylesheet" href="~/node_modules/vue-select/dist/vue-select.css" asp-append-version="true" />
<script src="~/node_modules/vue-select/dist/vue-select.umd.js" asp-append-version="true"></script>
```

## Vue開發流程

1. 建立頁面專用的ViewModel（ViewModel欄位盡量跟Entity一致，保持單純結構）

2. 執行 `TypeTransfer`， 將 ViewModel 轉成JS Class

3. 前端寫 html 與 vue，避免razor混雜進去

4. 用`fetch`與後端溝通資料

4. Post 整個 `ApplyData`，後端 ModelBinding ViewModel

5. Valid

6. Mapping ViewModel to Entity

7. SaveChange

8. 回傳，前端依據 `Http StatusCode` 顯示對應的 SweetAlert

## Vue元件

1. text -> 原生

2. select -> [vue-select](https://vue-select.org/)
   
3. datetime -> [vue-datepicker](https://vue3datepicker.com/)
   
4. checkbox -> 原生
   
5. radiobutton -> 原生
   
6. file -> 原生