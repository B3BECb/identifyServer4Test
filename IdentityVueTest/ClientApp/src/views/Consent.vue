<template>
	<div>{{this.Test}}</div>
</template>

<script lang = "ts">
	import { Component, Vue } from "vue-property-decorator";
	import Axios from "axios";

	@Component
	export default class Consent
		extends Vue
	{
		public Test:  string = "";

		async beforeMount()
		{
			const data = await Axios.create({
				url: "/consent"
			});

			console.log(data);
		}
	};
</script>

<style scoped>

</style>
