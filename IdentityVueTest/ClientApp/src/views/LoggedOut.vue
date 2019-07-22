<template>
	<div class = "page-header logged-out">
		<h1>
			Logout
			<small>You are now logged out</small>
		</h1>

		<div v-if = "!Model.PostLogoutRedirectUri">
			Click
			<a class = "PostLogoutRedirectUri"
			   :href = "Model.PostLogoutRedirectUri">
				here </a>
			to return to the <span>{{Model.ClientName}}</span> application.
		</div>

		<iframe v-if = "Model.SignOutIframeUrl"
				width = "0"
				height = "0"
				class = "signout"
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
			let data: any = await Axios({
				method: "get",
				url:    "/api/v1/consent",
				params: this.$route.query,
			});

			data = Capitalize(data.data);

			this.Model = data;
		}
	};
</script>

<style scoped>

</style>
