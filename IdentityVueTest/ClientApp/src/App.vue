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

	@include md-register-theme("error", (
			primary: md-get-palette-color(blue, A200), // The primary color of your application
			accent: md-get-palette-color(red, A200), // The accent or secondary color
			theme: light
	));

	@import "~vue-material/dist/theme/all";
</style>
