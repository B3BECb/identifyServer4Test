<template>
	<div>
		<div class="row page-header">
			<div class="col-sm-10">
				<h1>
					Client Application Access
				</h1>
				<div>Below is the list of applications you have given access to and the names of the resources they have access to.</div>
			</div>
		</div>
		<div class="row"
			 v-if="!Model.Grants.length">
			<div class="col-sm-8">
				<div class="alert alert-info">
					You have not given access to any applications
				</div>
			</div>
		</div>
		<div v-else
			 v-for="grant in Model.Grants"
			 class="row grant">
			<div class="col-sm-2">
				<img src="@grant.ClientLogoUrl"
					 v-if="grant.ClientLogoUrl">
			</div>
			<div class="col-sm-8">
				<div class="clientname">{{grant.ClientName}}</div>
				<div>
					<span class="created">Created:</span> {{grant.Created}}.ToString("yyyy-MM-dd")
				</div>
				<div v-if="grant.Expires">
					<span class="expires">Expires:</span> {{grant.Expires.Value}}.ToString("yyyy-MM-dd")
				</div>
				<div v-if="grant.IdentityGrantNames.length">
					<div class="granttype">Identity Grants</div>
					<ul>
						<li v-for="name in grant.IdentityGrantNames">{{name}}</li>
					</ul>
				</div>
				<div v-if="grant.ApiGrantNames.length">
					<div class="granttype">API Grants</div>
					<ul>
						<li v-for="name in grant.ApiGrantNames">{{name}}</li>
					</ul>
				</div>
			</div>
			<div class="col-sm-2">
				<form asp-action="Revoke">
					<input type="hidden" name="clientId" value="@grant.ClientId">
					<button class="btn btn-danger">Revoke Access</button>
				</form>
			</div>
		</div>
	</div>
</template>

<script lang = "ts">
	import { Component, Vue } from "vue-property-decorator";
	import Axios from "axios";
	import Capitalize from "@/assets/ts/helpers/KeysCapitalizer";

	@Component
	export default class Grants
		extends Vue
	{
		public Model: IGrantsViewModel | null = null;

		public async beforeMount()
		{
			let data: any = await Axios({
				method: "get",
				url:    "/api/v1/grants",
				params: this.$route.query,
			});

			data = Capitalize(data.data);

			this.Model = data as IGrantsViewModel;
		}
	}
</script>

<style scoped>

</style>
