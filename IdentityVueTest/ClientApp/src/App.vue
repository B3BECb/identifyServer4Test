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

	@import '~vue-material/dist/vue-material.min.css';

	@import "~vue-material/dist/theme/engine"; // Import the theme engine

	@include md-register-theme("default", (
			primary: md-get-palette-color(blue, A200), // The primary color of your application
			accent: md-get-palette-color(green, A200), // The accent or secondary color
			theme: light
	));

	@import "~vue-material/dist/theme/all";

	#app {
		&,*{
			font-family: Roboto, sans-serif;
			/*font-size: 14px;*/
			-webkit-font-smoothing: antialiased;
			-moz-osx-font-smoothing: grayscale;
			/*color: #2C3E50;*/
		}
		text-align: center;
	}
</style>
