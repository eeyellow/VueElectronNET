<script setup>
import SimpleDropDown from './components/SimpleDropDown.vue'
const VITE_BASEURL = import.meta.env.VITE_BASEURL
</script>

<template>
  <SimpleDropDown :url="`${VITE_BASEURL}/Api/Basic`" name="Name" id="ID" />
</template>

<style scoped>
.logo {
  height: 6em;
  padding: 1.5em;
  will-change: filter;
  transition: filter 300ms;
}
.logo:hover {
  filter: drop-shadow(0 0 2em #646cffaa);
}
.logo.vue:hover {
  filter: drop-shadow(0 0 2em #42b883aa);
}
</style>
