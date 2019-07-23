<template>
	<div class = "page-header logged-out">
		<h1>
			Logout
			<small>You are now logged out</small>
		</h1>

		<div v-if = "Model.PostLogoutRedirectUri">
			Click
			<a class = "PostLogoutRedirectUri"
			   :href = "Model.PostLogoutRedirectUri">
				here </a>
			to return to the <span>{{Model.ClientName}}</span> application.
		</div>

		<iframe v-if = "Model.SignOutIframeUrl"
				width = "0"
				height = "0"
				:src = "Model.SignOutIframeUrl"></iframe>
	</div>
</template>

<script lang="ts">
	import {Component, Vue} from "vue-property-decorator";
	import Axios from "axios";
	import Capitalize from "@/assets/ts/helpers/KeysCapitalizer";

	@Component
	export default class LoggedOut
		extends Vue
	{
		public Model: ILoggedOutViewModel | null = null;

		public async beforeMount()
		{
			const queryParams: any = this.$route.query;
			console.log(queryParams.PostLogoutRedirectUri);
			console.log(queryParams.ClientName);
			console.log(queryParams.SignOutIframeUrl);

			const model = {
				PostLogoutRedirectUri: queryParams.PostLogoutRedirectUri,
				ClientName: queryParams.ClientName,
				SignOutIframeUrl: queryParams.SignOutIframeUrl,
			};

			this.Model = model as ILoggedOutViewModel;
		}
	};
</script>

<style scoped>
	ifame
	{
		display: none;
		width: 0;
		height: 0;
	}
</style>
