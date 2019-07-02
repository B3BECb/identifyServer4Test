<template>
	<div id = "app">
		<router-view />
	</div>
</template>

<script lang = "ts">
	import { Component, Vue } from "vue-property-decorator";

	@Component({
		components:
			{},
	})

	export default class App
		extends Vue
	{

	}
</script>

<style lang = "scss">

	#app {
		&,*{
			font-family: Roboto, sans-serif;
			font-size: 14px;
			-webkit-font-smoothing: antialiased;
			-moz-osx-font-smoothing: grayscale;
			color: #2C3E50;
		}
		text-align: center;
	}
</style>
