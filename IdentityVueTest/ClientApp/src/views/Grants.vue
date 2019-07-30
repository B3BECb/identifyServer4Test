<template>
	<md-app md-mode = "reveal">
		<md-app-toolbar class = "md-primary">
			<md-button class = "md-icon-button"
					   :to = "{name: 'index'}">
				<md-icon>arrow_back</md-icon>
			</md-button>
			<span class = "md-title">Client Application Access</span>
		</md-app-toolbar>

		<md-app-content class = "md-layout md-alignment-center"
						v-if = "IsLoading">
			<md-progress-spinner md-mode = "indeterminate"></md-progress-spinner>
		</md-app-content>

		<md-app-content v-if = "!Model || IsLoadingError">
			<md-empty-state
					class = "md-accent md-theme-error"
					md-rounded
					md-icon = "sync_problem"
					md-label = "Cannot sync"
					md-description = "Unable to sync grants.">
			</md-empty-state>
		</md-app-content>

		<md-app-content v-else-if = "Model.Grants.length && !IsLoading">
			<md-card v-for = "grant in Model.Grants">
				<md-card-header class = "md-title">
					{{grant.ClientName}}
				</md-card-header>

				<md-card-media v-if = "grant.ClientLogoUrl">
					<img :src = "grant.ClientLogoUrl">
				</md-card-media>

				<md-card-content>
					<div>
						<span class = "md-subheading">Created:</span>
						{{DateTime.fromISO(grant.Created).toFormat("dd.MM.yyyy")}}
					</div>
					<div v-if = "grant.Expires">
						<span class = "md-subheading">Expires:</span>
						{{DateTime.fromISO(grant.Expires.Value).toFormat("dd.MM.yyyy")}}
					</div>
					<div v-if = "grant.IdentityGrantNames.length">
						<div class = "md-subheading">Identity Grants</div>
						<ul>
							<li v-for = "name in grant.IdentityGrantNames">{{name}}</li>
						</ul>
					</div>
					<div v-if = "grant.ApiGrantNames.length">
						<div class = "md-subheading">API Grants</div>
						<ul>
							<li v-for = "name in grant.ApiGrantNames">{{name}}</li>
						</ul>
					</div>
				</md-card-content>

				<md-card-actions>
					<md-button class = "md-raised md-accent md-theme-error"
							   @click = "Revoke(grant.ClientId)">
						Revoke
					</md-button>
				</md-card-actions>
			</md-card>
		</md-app-content>

		<md-app-content v-else>
			<md-empty-state
					md-rounded
					md-icon = "apps"
					md-label = "No grants"
					md-description = "You have not given access to any applications.">
			</md-empty-state>
		</md-app-content>
	</md-app>
</template>

<script lang = "ts">
	import { Component, Vue } from "vue-property-decorator";
	import Axios from "axios";
	import Capitalize from "@/assets/ts/helpers/KeysCapitalizer";
	import {
		MdButton,
		MdCard,
		MdToolbar,
		MdApp,
		MdIcon,
		MdContent,
		MdEmptyState,
		MdProgress,
		// @ts-ignore
	} from "vue-material/dist/components";
	import { DateTime } from "luxon";

	Vue.use(MdButton);
	Vue.use(MdCard);
	Vue.use(MdToolbar);
	Vue.use(MdApp);
	Vue.use(MdIcon);
	Vue.use(MdContent);
	Vue.use(MdEmptyState);
	Vue.use(MdProgress);

	@Component
	export default class Grants
		extends Vue
	{
		public Model: IGrantsViewModel | null = null;

		public DateTime: any = DateTime;

		public IsLoading: boolean      = false;
		public IsLoadingError: boolean = false;

		public async beforeMount()
		{
			this.IsLoading = true;

			try
			{
				await this.GetGrants();
			}
			catch(exc)
			{
				this.IsLoadingError = true;
			}

			this.IsLoading = false;
		}

		public async Revoke(clientId: string)
		{
			try
			{
				this.IsLoading = true;

				await Axios({
					method: "post",
					url:    "/api/v1/grants",
					params: { clientId: clientId },
				});

				await this.GetGrants();
				this.IsLoading = false;
			}
			catch(exc)
			{
				this.IsLoadingError = true;
				this.IsLoading      = false;
			}
		}

		private async GetGrants()
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

<style scoped
	   lang = "scss">
	.md-app
	{
		height: 100vh;
	}

	.md-card {
		width: 320px;
		margin: 4px;
		display: inline-block;
		vertical-align: top;
	}

</style>
