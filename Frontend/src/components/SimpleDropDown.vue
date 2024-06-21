<script setup>
import { onMounted, reactive, toRefs } from 'vue'

const props = defineProps({
  url: String,
  name: String,
  id: String
})

const data = reactive({
    departments: [
      { "Name": "請選擇" }
    ]
})

onMounted(async () => {
  let response = await fetch(props.url, {
    method: "GET"
  })
  if (response.ok) {
    data.departments.push(...await response.json())
  }
})

</script>

<template>
  <select>
    <option v-for="department in data.departments" 
      :value="department[props.id]">
      {{ department[props.name] }}
    </option>
  </select>
</template>

<style scoped>

</style>
