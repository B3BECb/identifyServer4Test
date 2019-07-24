<template>
	<div>
		<div>
			<div>Welcome to Mallenom Systems OIDC server</div>
			<md-list>
				<md-list-item :to="{name: 'grants'}">Stored grants</md-list-item>
				<md-list-item :to="{name: 'logout'}">Logout</md-list-item>
			</md-list>
		</div>
	</div>
</template>

<script lang = "ts">
	import { Component, Vue } from "vue-property-decorator";
	import {
		MdList,
		// @ts-ignore
	} from "vue-material/dist/components";

	Vue.use(MdList);

	@Component
	export default class Index
		extends Vue
	{

	};
</script>

<style scoped>

</style>
