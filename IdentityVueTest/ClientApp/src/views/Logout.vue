<template>
	<div class = "logout-page">
		<div class = "page-header">
			<h1>Logout</h1>
		</div>

		<div class = "row">
			<div class = "col-sm-6">
				<p>Would you like to logout of IdentityServer?</p>
				<form asp-action = "Logout">
					<input type = "hidden"
						   name = "logoutId"
						   :value = "Model.LogoutId" />
					<fieldset>
						<div class = "form-group">
							<button class = "btn btn-primary">Yes</button>
						</div>
					</fieldset>
				</form>
			</div>
		</div>
	</div>
</template>

<script lang="ts">
	import {Component, Vue} from "vue-property-decorator";
	import Axios from "axios";
	import Capitalize from "@/assets/ts/helpers/KeysCapitalizer";

	@Component
	export default class Logout
		extends Vue
	{
		public Model: ILogoutViewModel | null = null;
		public XSRF: string                    = "";

		public async beforeMount()
		{
			let data: any = await Axios({
				method: "get",
				url:    "/api/v1/authorization/logout",
				params: this.$route.query,
			});

			data = Capitalize(data.data);

			this.Model = data;
		}
	};
</script>

<style scoped>

</style>
