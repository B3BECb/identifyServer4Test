<template>
	<div class = "logout-page">
		<div class = "page-header">
			<h1>Logout</h1>
		</div>

		<div class = "row">
			<div class = "col-sm-6">
				<p>Would you like to logout of IdentityServer?</p>
				<form action="/account/logout"
					  method="post">
					<input v-model = "XSRF"
						   type="hidden"
						   name = "XSRF-TOKEN-FIELD" />
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

	@Component
	export default class Logout
		extends Vue
	{
		public Model: ILogoutViewModel | null = null;
		public XSRF: string                    = "";

		public async beforeMount()
		{
			const queryParams: any = this.$route.query;

			const model = {
				LogoutId: queryParams.logoutId || queryParams.LogoutId,
			};

			this.Model = model as ILogoutViewModel;

			this.XSRF = this.$cookies.get("XSRF-TOKEN");
		}
	};
</script>

<style scoped>

</style>
