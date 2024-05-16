# Vue開發說明

前端套件都是安裝在www/node_modules下，統一使用npm管理。

## 1. [Vue](https://vuejs.org/guide/quick-start.html)

安裝

```sh
npm install vue
```

```html
<script src="~/node_modules/vue/dist/vue.global.js" asp-append-version="true"></script>
```

## 2. 列表套件: [vue-lcgrid](https://www.npmjs.com/package/@linkchain/vue-lcgrid)

安裝

```sh
npm install @linkchain/vue-lcgrid@0.0.13
```

```html
<link rel="stylesheet" href="~/node_modules/@@linkchain/vue-lcgrid/dist/lcGridVue.css" asp-append-version="true" />
<script src="~/node_modules/@@linkchain/vue-lcgrid/dist/lcGridVue.js" asp-append-version="true"></script>
```

## 3. 日期時間套件: [vue-datepicker](https://vue3datepicker.com/)

安裝

```sh
npm install @vuepic/vue-datepicker
```

```html
<link rel="stylesheet" href="~/node_modules/@@vuepic/vue-datepicker/dist/main.css" asp-append-version="true" />
<script src="~/node_modules/@@vuepic/vue-datepicker/dist/vue-datepicker.iife.js" asp-append-version="true"></script>
```