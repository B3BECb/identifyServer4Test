<template>
	<div class="formContainer">
		<form class = "md-layout"
		  @submit = "submit"
		  action = "/api/v1/consent"
		  method = "post">
		<md-card class = "md-layout-item"
				 v-if = "Model">
			<md-card-header>
				<div>
					<span class = "name md-title">{{Model.ClientName}}</span>
					<span class = "md-subheading"> is requesting your permission</span>
				</div>
			</md-card-header>
			<md-card-content>
				<div class = "md-layout md-gutter md-alignment-left">
					<div class = "md-hide">
						<input type = "hidden"
							   v-model = "XSRF"
							   name = "XSRF-TOKEN-FIELD" />
					</div>
					<div class = "md-hide">
						<input type = "hidden"
							   v-model = "ReturnUrl"
							   name = "returnUrl" />
					</div>
					<div class = "md-layout-item md-small-size-100"
						 v-if = "Model.IdentityScopes.length">

						<md-list>
							<md-subheader class = "md-layout md-alignment-center-left">
								<!--<span class="md-layout-item">-->
								<!--<md-icon class = "icon">account_circle</md-icon>-->
								<!--</span>-->
								<span class = "md-layout-item">Personal Information</span>
							</md-subheader>
							<md-list-item v-for = "scope in Model.IdentityScopes">
								<md-checkbox name = "ScopesConsented"
											 v-bind:id = "'scopes_' + scope.Name"
											 v-model = "scope.Checked"
											 v-bind:disabled = "scope.Required" />
								<span class = "md-list-item-text">
									<span>
										<span>
											<span>{{scope.DisplayName}}</span>
											<span v-if = "scope.Required"
												  class = "md-caption"><em> (required)</em></span>
										</span>
									</span>
									<span v-if = "scope.Description"
										  class = "md-caption">
										<label v-bind:for = "'scopes' + scope.Name">{{scope.Description}}</label>
									</span>
								</span>
								<md-icon v-if = "scope.Emphasize">error</md-icon>
								<input v-if = "scope.Required || scope.Checked"
									   type = "hidden"
									   name = "ScopesConsented"
									   v-bind:value = "scope.Name" />
							</md-list-item>

							<md-divider v-if = "Model.ResourceScopes.length"></md-divider>

							<md-subheader class = "md-layout md-alignment-center-left"
										  v-if = "Model.ResourceScopes.length">
								<!--<span class="md-layout-item">-->
								<!--<md-icon class = "icon">account_circle</md-icon>-->
								<!--</span>-->
								<span class = "md-layout-item">Application Access</span>
							</md-subheader>

							<md-list-item v-if = "Model.ResourceScopes.length"
										  v-for = "scope in Model.ResourceScopes">
								<md-checkbox name = "Md_ScopesConsented"
											 v-model = "scope.Checked"
											 :id = "'scopes_' + scope.Name"
											 :disabled = "scope.Required" />
								<span class = "md-list-item-text">
									<span>
										<span>
											<span>{{scope.DisplayName}}</span>
											<span v-if = "scope.Required"
												  class = "md-caption"><em> (required)</em></span>
										</span>
									</span>
									<span v-if = "scope.Description"
										  class = "md-caption">
										<label v-bind:for = "'scopes' + scope.Name">{{scope.Description}}</label>
									</span>
								</span>
								<md-icon v-if = "scope.Emphasize">error</md-icon>
								<input v-if = "scope.Required || scope.Checked"
									   type = "hidden"
									   name = "ScopesConsented"
									   :value = "scope.Name" />
							</md-list-item>
						</md-list>
					</div>
				</div>
			</md-card-content>
			<md-card-actions md-alignment = "space-between">
				<span>
					<md-checkbox style = "margin-left: 24px"
								 v-model = "Model.RememberConsent"
								 v-if = "Model.AllowRememberConsent">Remember My Decision</md-checkbox>
					<input type = "hidden"
						   name = "RememberConsent"
						   :value = Model.RememberConsent>
				</span>
				<span>
					<md-button name = "button"
							   value = "yes"
							   type = "submit"
							   class = "md-raised md-primary"
							   autofocus>Allow
					</md-button>
					<md-button name = "button"
							   value = "no"
							   type = "submit"
							   class = "md-dense md-primary"
							   autofocus>Cancel
				</md-button>
				</span>
			</md-card-actions>
		</md-card>
	</form>
	</div>
</template>

<script lang = "ts">
	import { Component, Vue } from "vue-property-decorator";
	import Axios from "axios";
	import Capitalize from "@/assets/ts/helpers/KeysCapitalizer";
	import {
		MdButton,
		MdContent,
		MdCard,
		MdCheckbox,
		MdIcon,
		MdList,
		MdSubheader,
		MdDivider,
		// @ts-ignore
	} from "vue-material/dist/components";

	Vue.use(MdButton);
	Vue.use(MdContent);
	Vue.use(MdCard);
	Vue.use(MdCheckbox);
	Vue.use(MdIcon);
	Vue.use(MdList);
	Vue.use(MdSubheader);
	Vue.use(MdDivider);

	@Component
	export default class Consent
		extends Vue
	{
		public Model: IConsentViewModel | null = null;
		public ReturnUrl: string               = "";
		public XSRF: string                    = "";

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

		public submit(e: Event)
		{
			const request = (this.$route.query as any);

			this.ReturnUrl = request.ReturnUrl || request.returnUrl || "";

			this.XSRF = this.$cookies.get("XSRF-TOKEN");

			return true;
		}
	}
</script>

<style scoped
	   lang = "scss">

	@import '~material-icons-font/sass/variables';
	@import '~material-icons-font/sass/mixins';

	$MaterialIcons_FontPath: "~material-icons-font/fonts";
	@import '~material-icons-font/sass/main';
	@import '~material-icons-font/sass/Regular';
	@import '~material-icons-font/sass/sizing';
	@import '~material-icons-font/sass/coloring';

	.formContainer
	{
		display: grid;

		height: 100vh;
		width: 100vw;

		grid-template-areas: ". . ." ". form ." ". . .";

		grid-template-columns: 1fr 1fr 1fr;
		grid-template-rows: 1fr 1fr 1fr;

		background: url(data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxOTIwIiBoZWlnaHQ9IjEwODAiPgogIDxwYXRoIGZpbGw9IiNmM2YzZjMiIHN0cm9rZT0iI2YzZjNmMyIgc3Ryb2tlLXdpZHRoPSIxLjUxIiBkPSJNMTQ0LTQ5Nmw2MjcgMTY3IDI1OSA2eiIvPgogIDxwYXRoIGZpbGw9IiNlM2UzZTMiIHN0cm9rZT0iI2UzZTNlMyIgc3Ryb2tlLXdpZHRoPSIxLjUxIiBkPSJNNDczIDE1MjZsLTcxMCAxNDQgMTU0OS02NHoiLz4KICA8cGF0aCBmaWxsPSIjZWVlIiBzdHJva2U9IiNlZWUiIHN0cm9rZS13aWR0aD0iMS41MSIgZD0iTTEwMzAtMzIzaDU5MWwzODMtMTU5eiIvPgogIDxwYXRoIGZpbGw9IiNmMWYxZjEiIHN0cm9rZT0iI2YxZjFmMSIgc3Ryb2tlLXdpZHRoPSIxLjUxIiBkPSJNMTQ0LTQ5Nmw4ODYgMTczIDk3NC0xNTl6Ii8+CiAgPHBhdGggZmlsbD0iI2UwZTBlMCIgc3Ryb2tlPSIjZTBlMGUwIiBzdHJva2Utd2lkdGg9IjEuNTEiIGQ9Ik0yMjMxIDQ5NWwtNjIgMjA1IDEwNCAzMTJ6Ii8+CiAgPHBhdGggZmlsbD0iI2U2ZTZlNiIgc3Ryb2tlPSIjZTZlNmU2IiBzdHJva2Utd2lkdGg9IjEuNTEiIGQ9Ik0xOTc4LTQybDI1MyA1MzcgNjEtNzQ1eiIvPgogIDxwYXRoIGZpbGw9IiNlZmVmZWYiIHN0cm9rZT0iI2VmZWZlZiIgc3Ryb2tlLXdpZHRoPSIxLjUxIiBkPSJNMTQ0LTQ5NmwxODYwIDE0IDUzNC05MnoiLz4KICA8cGF0aCBmaWxsPSIjZjVmNWY1IiBzdHJva2U9IiNmNWY1ZjUiIHN0cm9rZS13aWR0aD0iMS41MSIgZD0iTS01OTgtNTYxbDc0MiA2NSAyMzk0LTc4eiIvPgogIDxwYXRoIGZpbGw9IiNlMGUwZTAiIHN0cm9rZT0iI2UwZTBlMCIgc3Ryb2tlLXdpZHRoPSIxLjUxIiBkPSJNMjI3MyAxMDEybC0xOSA0NDkgMjg0LTIwMzV6Ii8+CiAgPHBhdGggZmlsbD0iI2VhZWFlYSIgc3Ryb2tlPSIjZWFlYWVhIiBzdHJva2Utd2lkdGg9IjEuNTEiIGQ9Ik0yMDA0LTQ4MmwyODggMjMyIDI0Ni0zMjR6Ii8+CiAgPHBhdGggZmlsbD0iI2U2ZTZlNiIgc3Ryb2tlPSIjZTZlNmU2IiBzdHJva2Utd2lkdGg9IjEuNTEiIGQ9Ik0yMjkyLTI1MGwtNjEgNzQ1IDMwNy0xMDY5eiIvPgogIDxwYXRoIGZpbGw9IiNlM2UzZTMiIHN0cm9rZT0iI2UzZTNlMyIgc3Ryb2tlLXdpZHRoPSIxLjUxIiBkPSJNMjIzMSA0OTVsNDIgNTE3IDI2NS0xNTg2eiIvPgogIDxwYXRoIGZpbGw9IiNkZGQiIHN0cm9rZT0iI2RkZCIgc3Ryb2tlLXdpZHRoPSIxLjUxIiBkPSJNMTg0OSAxMDAzbC0xMDMgNDEyIDIyOSAxNDF6Ii8+CiAgPHBhdGggZmlsbD0iI2U0ZTRlNCIgc3Ryb2tlPSIjZTRlNGU0IiBzdHJva2Utd2lkdGg9IjEuNTEiIGQ9Ik0xOTc4LTQybC05MyA1MzAgMzQ2IDd6Ii8+CiAgPHBhdGggZmlsbD0iI2RjZGNkYyIgc3Ryb2tlPSIjZGNkY2RjIiBzdHJva2Utd2lkdGg9IjEuNTEiIGQ9Ik0xODQ5IDEwMDNsMTI2IDU1MyAyNzktOTV6Ii8+CiAgPHBhdGggZmlsbD0iI2RkZCIgc3Ryb2tlPSIjZGRkIiBzdHJva2Utd2lkdGg9IjEuNTEiIGQ9Ik0xODQ5IDEwMDNsNDA1IDQ1OCAxOS00NDl6Ii8+CiAgPHBhdGggZmlsbD0iI2RmZGZkZiIgc3Ryb2tlPSIjZGZkZmRmIiBzdHJva2Utd2lkdGg9IjEuNTEiIGQ9Ik0yMTY5IDcwMGwtMzIwIDMwMyA0MjQgOXoiLz4KICA8cGF0aCBmaWxsPSIjZWFlYWVhIiBzdHJva2U9IiNlYWVhZWEiIHN0cm9rZS13aWR0aD0iMS41MSIgZD0iTTIwMDQtNDgybC0yNiA0NDAgMzE0LTIwOHoiLz4KICA8cGF0aCBmaWxsPSIjZTJlMmUyIiBzdHJva2U9IiNlMmUyZTIiIHN0cm9rZS13aWR0aD0iMS41MSIgZD0iTTE4ODUgNDg4bDI4NCAyMTIgNjItMjA1eiIvPgogIDxwYXRoIGZpbGw9IiNlMmUyZTIiIHN0cm9rZT0iI2UyZTJlMiIgc3Ryb2tlLXdpZHRoPSIxLjUxIiBkPSJNMTg4NSA0ODhsLTgzIDEzMyAzNjcgNzl6Ii8+CiAgPHBhdGggZmlsbD0iI2UxZTFlMSIgc3Ryb2tlPSIjZTFlMWUxIiBzdHJva2Utd2lkdGg9IjEuNTEiIGQ9Ik0xODAyIDYyMWw0NyAzODIgMzIwLTMwM3oiLz4KICA8cGF0aCBmaWxsPSIjZGZkZmRmIiBzdHJva2U9IiNkZmRmZGYiIHN0cm9rZS13aWR0aD0iMS41MSIgZD0iTTE2ODUgMTEzOWw2MSAyNzYgMTAzLTQxMnoiLz4KICA8cGF0aCBmaWxsPSIjZTJlMmUyIiBzdHJva2U9IiNlMmUyZTIiIHN0cm9rZS13aWR0aD0iMS41MSIgZD0iTTE4MDIgNjIxbC02MiA2OCAxMDkgMzE0eiIvPgogIDxwYXRoIGZpbGw9IiNkY2RjZGMiIHN0cm9rZT0iI2RjZGNkYyIgc3Ryb2tlLXdpZHRoPSIxLjUxIiBkPSJNMTc0NiAxNDE1bC00MzQgMTkxIDY2My01MHoiLz4KICA8cGF0aCBmaWxsPSIjZTdlN2U3IiBzdHJva2U9IiNlN2U3ZTciIHN0cm9rZS13aWR0aD0iMS41MSIgZD0iTTE2MTcgMTcybDI2OCAzMTYgOTMtNTMweiIvPgogIDxwYXRoIGZpbGw9IiNlYmViZWIiIHN0cm9rZT0iI2ViZWJlYiIgc3Ryb2tlLXdpZHRoPSIxLjUxIiBkPSJNMTYyMS0zMjNsMzU3IDI4MSAyNi00NDB6Ii8+CiAgPHBhdGggZmlsbD0iI2VhZWFlYSIgc3Ryb2tlPSIjZWFlYWVhIiBzdHJva2Utd2lkdGg9IjEuNTEiIGQ9Ik0xNjIxLTMyM2wtNCA0OTUgMzYxLTIxNHoiLz4KICA8cGF0aCBmaWxsPSIjZWJlYmViIiBzdHJva2U9IiNlYmViZWIiIHN0cm9rZS13aWR0aD0iMS41MSIgZD0iTTE0MTAtNDlsMjA3IDIyMSA0LTQ5NXoiLz4KICA8cGF0aCBmaWxsPSIjZTVlNWU1IiBzdHJva2U9IiNlNWU1ZTUiIHN0cm9rZS13aWR0aD0iMS41MSIgZD0iTTE2MTcgMTcybDE4NSA0NDkgODMtMTMzeiIvPgogIDxwYXRoIGZpbGw9IiNlNWU1ZTUiIHN0cm9rZT0iI2U1ZTVlNSIgc3Ryb2tlLXdpZHRoPSIxLjUxIiBkPSJNMTYxNyAxNzJsMTIzIDUxNyA2Mi02OHoiLz4KICA8cGF0aCBmaWxsPSIjZTFlMWUxIiBzdHJva2U9IiNlMWUxZTEiIHN0cm9rZS13aWR0aD0iMS41MSIgZD0iTTE3NDAgNjg5bC01NSA0NTAgMTY0LTEzNnoiLz4KICA8cGF0aCBmaWxsPSIjZTZlNmU2IiBzdHJva2U9IiNlNmU2ZTYiIHN0cm9rZS13aWR0aD0iMS41MSIgZD0iTTE2MTcgMTcybC0zOTMgNDUxIDUxNiA2NnoiLz4KICA8cGF0aCBmaWxsPSIjZTJlMmUyIiBzdHJva2U9IiNlMmUyZTIiIHN0cm9rZS13aWR0aD0iMS41MSIgZD0iTTEyNDQgOTYybDQ0MSAxNzcgNTUtNDUweiIvPgogIDxwYXRoIGZpbGw9IiNlNWU1ZTUiIHN0cm9rZT0iI2U1ZTVlNSIgc3Ryb2tlLXdpZHRoPSIxLjUxIiBkPSJNMTIyNCA2MjNsMjAgMzM5IDQ5Ni0yNzN6Ii8+CiAgPHBhdGggZmlsbD0iI2RmZGZkZiIgc3Ryb2tlPSIjZGZkZmRmIiBzdHJva2Utd2lkdGg9IjEuNTEiIGQ9Ik0xNjg1IDExMzlsLTM3MyA0NjcgNDM0LTE5MXoiLz4KICA8cGF0aCBmaWxsPSIjZTFlMWUxIiBzdHJva2U9IiNlMWUxZTEiIHN0cm9rZS13aWR0aD0iMS41MSIgZD0iTTEyNDQgOTYybDY4IDY0NCAzNzMtNDY3eiIvPgogIDxwYXRoIGZpbGw9IiNlYWVhZWEiIHN0cm9rZT0iI2VhZWFlYSIgc3Ryb2tlLXdpZHRoPSIxLjUxIiBkPSJNMTQxMC00OWwtMzQyIDE3NSA1NDkgNDZ6Ii8+CiAgPHBhdGggZmlsbD0iI2U5ZTllOSIgc3Ryb2tlPSIjZTllOWU5IiBzdHJva2Utd2lkdGg9IjEuNTEiIGQ9Ik0xMDY4IDEyNmwxNTYgNDk3IDM5My00NTF6Ii8+CiAgPHBhdGggZmlsbD0iI2VkZWRlZCIgc3Ryb2tlPSIjZWRlZGVkIiBzdHJva2Utd2lkdGg9IjEuNTEiIGQ9Ik0xMDMwLTMyM2wzODAgMjc0IDIxMS0yNzR6Ii8+CiAgPHBhdGggZmlsbD0iI2VhZWFlYSIgc3Ryb2tlPSIjZWFlYWVhIiBzdHJva2Utd2lkdGg9IjEuNTEiIGQ9Ik0xMDY4IDEyNmwtNzkgODIgMjM1IDQxNXoiLz4KICA8cGF0aCBmaWxsPSIjZTRlNGU0IiBzdHJva2U9IiNlNGU0ZTQiIHN0cm9rZS13aWR0aD0iMS41MSIgZD0iTTkzNyA5NjJsLTE4NSA0MDkgNDkyLTQwOXoiLz4KICA8cGF0aCBmaWxsPSIjZTJlMmUyIiBzdHJva2U9IiNlMmUyZTIiIHN0cm9rZS13aWR0aD0iMS41MSIgZD0iTTEyNDQgOTYybC00OTIgNDA5IDU2MCAyMzV6Ii8+CiAgPHBhdGggZmlsbD0iI2VkZWRlZCIgc3Ryb2tlPSIjZWRlZGVkIiBzdHJva2Utd2lkdGg9IjEuNTEiIGQ9Ik0xMDMwLTMyM2wzOCA0NDkgMzQyLTE3NXoiLz4KICA8cGF0aCBmaWxsPSIjZTVlNWU1IiBzdHJva2U9IiNlNWU1ZTUiIHN0cm9rZS13aWR0aD0iMS41MSIgZD0iTTEyMjQgNjIzTDkzNyA5NjJoMzA3eiIvPgogIDxwYXRoIGZpbGw9IiNlMmUyZTIiIHN0cm9rZT0iI2UyZTJlMiIgc3Ryb2tlLXdpZHRoPSIxLjUxIiBkPSJNNzUyIDEzNzFsLTI3OSAxNTUgODM5IDgweiIvPgogIDxwYXRoIGZpbGw9IiNlYWVhZWEiIHN0cm9rZT0iI2VhZWFlYSIgc3Ryb2tlLXdpZHRoPSIxLjUxIiBkPSJNOTg5IDIwOEw3ODMgNDAxbDQ0MSAyMjJ6Ii8+CiAgPHBhdGggZmlsbD0iI2U3ZTdlNyIgc3Ryb2tlPSIjZTdlN2U3IiBzdHJva2Utd2lkdGg9IjEuNTEiIGQ9Ik03NDggNzg5bDE4OSAxNzMgMjg3LTMzOXoiLz4KICA8cGF0aCBmaWxsPSIjZTllOWU5IiBzdHJva2U9IiNlOWU5ZTkiIHN0cm9rZS13aWR0aD0iMS41MSIgZD0iTTc4MyA0MDFsLTM1IDM4OCA0NzYtMTY2eiIvPgogIDxwYXRoIGZpbGw9IiNmMGYwZjAiIHN0cm9rZT0iI2YwZjBmMCIgc3Ryb2tlLXdpZHRoPSIxLjUxIiBkPSJNNzcxLTMyOWwtMTIgNDU0IDI3MS00NDh6Ii8+CiAgPHBhdGggZmlsbD0iI2VlZSIgc3Ryb2tlPSIjZWVlIiBzdHJva2Utd2lkdGg9IjEuNTEiIGQ9Ik0xMDMwLTMyM0w3NTkgMTI1bDMwOSAxeiIvPgogIDxwYXRoIGZpbGw9IiNlY2VjZWMiIHN0cm9rZT0iI2VjZWNlYyIgc3Ryb2tlLXdpZHRoPSIxLjUxIiBkPSJNNzU5IDEyNWwyMzAgODMgNzktODJ6Ii8+CiAgPHBhdGggZmlsbD0iI2U1ZTVlNSIgc3Ryb2tlPSIjZTVlNWU1IiBzdHJva2Utd2lkdGg9IjEuNTEiIGQ9Ik00ODEgMTEwNWwyNzEgMjY2IDE4NS00MDl6Ii8+CiAgPHBhdGggZmlsbD0iI2VjZWNlYyIgc3Ryb2tlPSIjZWNlY2VjIiBzdHJva2Utd2lkdGg9IjEuNTEiIGQ9Ik03NTkgMTI1bDI0IDI3NiAyMDYtMTkzeiIvPgogIDxwYXRoIGZpbGw9IiNlN2U3ZTciIHN0cm9rZT0iI2U3ZTdlNyIgc3Ryb2tlLXdpZHRoPSIxLjUxIiBkPSJNNzQ4IDc4OWwtMjY3IDMxNiA0NTYtMTQzeiIvPgogIDxwYXRoIGZpbGw9IiNlOGU4ZTgiIHN0cm9rZT0iI2U4ZThlOCIgc3Ryb2tlLXdpZHRoPSIxLjUxIiBkPSJNNTE5IDc4MmwtMzggMzIzIDI2Ny0zMTZ6Ii8+CiAgPHBhdGggZmlsbD0iI2YxZjFmMSIgc3Ryb2tlPSIjZjFmMWYxIiBzdHJva2Utd2lkdGg9IjEuNTEiIGQ9Ik0zMzUtMTQ4bDQyNCAyNzMgMTItNDU0eiIvPgogIDxwYXRoIGZpbGw9IiNmNGY0ZjQiIHN0cm9rZT0iI2Y0ZjRmNCIgc3Ryb2tlLXdpZHRoPSIxLjUxIiBkPSJNMTcyLTQyOGwxNjMgMjgwIDQzNi0xODF6Ii8+CiAgPHBhdGggZmlsbD0iI2Y2ZjZmNiIgc3Ryb2tlPSIjZjZmNmY2IiBzdHJva2Utd2lkdGg9IjEuNTEiIGQ9Ik0xNDQtNDk2bDI4IDY4IDU5OSA5OXoiLz4KICA8cGF0aCBmaWxsPSIjZWFlYWVhIiBzdHJva2U9IiNlYWVhZWEiIHN0cm9rZS13aWR0aD0iMS41MSIgZD0iTTUxOSA3ODJsMjI5IDcgMzUtMzg4eiIvPgogIDxwYXRoIGZpbGw9IiNlZWUiIHN0cm9rZT0iI2VlZSIgc3Ryb2tlLXdpZHRoPSIxLjUxIiBkPSJNNzU5IDEyNUwyMDIgMjU5bDU4MSAxNDJ6Ii8+CiAgPHBhdGggZmlsbD0iI2VjZWNlYyIgc3Ryb2tlPSIjZWNlY2VjIiBzdHJva2Utd2lkdGg9IjEuNTEiIGQ9Ik0yMDIgMjU5bDMxNyA1MjMgMjY0LTM4MXoiLz4KICA8cGF0aCBmaWxsPSIjZjBmMGYwIiBzdHJva2U9IiNmMGYwZjAiIHN0cm9rZS13aWR0aD0iMS41MSIgZD0iTTMzNS0xNDhMMjAyIDI1OWw1NTctMTM0eiIvPgogIDxwYXRoIGZpbGw9IiNlNWU1ZTUiIHN0cm9rZT0iI2U1ZTVlNSIgc3Ryb2tlLXdpZHRoPSIxLjUxIiBkPSJNNDgxIDExMDVsLTggNDIxIDI3OS0xNTV6Ii8+CiAgPHBhdGggZmlsbD0iI2U4ZThlOCIgc3Ryb2tlPSIjZThlOGU4IiBzdHJva2Utd2lkdGg9IjEuNTEiIGQ9Ik0tMTc5IDEwNDZsNjUyIDQ4MCA4LTQyMXoiLz4KICA8cGF0aCBmaWxsPSIjZWNlY2VjIiBzdHJva2U9IiNlY2VjZWMiIHN0cm9rZS13aWR0aD0iMS41MSIgZD0iTS01IDU5M2wtMTc0IDQ1MyA2NjAgNTl6Ii8+CiAgPHBhdGggZmlsbD0iI2ViZWJlYiIgc3Ryb2tlPSIjZWJlYmViIiBzdHJva2Utd2lkdGg9IjEuNTEiIGQ9Ik0tNSA1OTNsNDg2IDUxMiAzOC0zMjN6Ii8+CiAgPHBhdGggZmlsbD0iI2VlZSIgc3Ryb2tlPSIjZWVlIiBzdHJva2Utd2lkdGg9IjEuNTEiIGQ9Ik0yMDIgMjU5TC01IDU5M2w1MjQgMTg5eiIvPgogIDxwYXRoIGZpbGw9IiNlOGU4ZTgiIHN0cm9rZT0iI2U4ZThlOCIgc3Ryb2tlLXdpZHRoPSIxLjUxIiBkPSJNLTE3OSAxMDQ2bC01OCA2MjQgNzEwLTE0NHoiLz4KICA8cGF0aCBmaWxsPSIjZjNmM2YzIiBzdHJva2U9IiNmM2YzZjMiIHN0cm9rZS13aWR0aD0iMS41MSIgZD0iTS0xMDktMjNsMzExIDI4MiAxMzMtNDA3eiIvPgogIDxwYXRoIGZpbGw9IiNmNWY1ZjUiIHN0cm9rZT0iI2Y1ZjVmNSIgc3Ryb2tlLXdpZHRoPSIxLjUxIiBkPSJNMTcyLTQyOEwtMTA5LTIzbDQ0NC0xMjV6Ii8+CiAgPHBhdGggZmlsbD0iI2VlZSIgc3Ryb2tlPSIjZWVlIiBzdHJva2Utd2lkdGg9IjEuNTEiIGQ9Ik0tMjQ1IDc3Mmw2NiAyNzRMLTUgNTkzeiIvPgogIDxwYXRoIGZpbGw9IiNmMWYxZjEiIHN0cm9rZT0iI2YxZjFmMSIgc3Ryb2tlLXdpZHRoPSIxLjUxIiBkPSJNLTk2IDI5N2w5MSAyOTYgMjA3LTMzNHoiLz4KICA8cGF0aCBmaWxsPSIjZjNmM2YzIiBzdHJva2U9IiNmM2YzZjMiIHN0cm9rZS13aWR0aD0iMS41MSIgZD0iTS0xMDktMjNsMTMgMzIwIDI5OC0zOHoiLz4KICA8cGF0aCBmaWxsPSIjZjdmN2Y3IiBzdHJva2U9IiNmN2Y3ZjciIHN0cm9rZS13aWR0aD0iMS41MSIgZD0iTTE0NC00OTZMLTEwOS0yM2wyODEtNDA1eiIvPgogIDxwYXRoIGZpbGw9IiNmOWY5ZjkiIHN0cm9rZT0iI2Y5ZjlmOSIgc3Ryb2tlLXdpZHRoPSIxLjUxIiBkPSJNLTU5OC01NjFsNDg5IDUzOCAyNTMtNDczeiIvPgogIDxwYXRoIGZpbGw9IiNlYmViZWIiIHN0cm9rZT0iI2ViZWJlYiIgc3Ryb2tlLXdpZHRoPSIxLjUxIiBkPSJNLTU1NCAxMzkybDMxNyAyNzggNTgtNjI0eiIvPgogIDxwYXRoIGZpbGw9IiNmNGY0ZjQiIHN0cm9rZT0iI2Y0ZjRmNCIgc3Ryb2tlLXdpZHRoPSIxLjUxIiBkPSJNLTEwOS0yM2wtMjQ4IDE1MSAyNjEgMTY5eiIvPgogIDxwYXRoIGZpbGw9IiNmMGYwZjAiIHN0cm9rZT0iI2YwZjBmMCIgc3Ryb2tlLXdpZHRoPSIxLjUxIiBkPSJNLTk2IDI5N2wtMTQ5IDQ3NUwtNSA1OTN6Ii8+CiAgPHBhdGggZmlsbD0iI2VkZWRlZCIgc3Ryb2tlPSIjZWRlZGVkIiBzdHJva2Utd2lkdGg9IjEuNTEiIGQ9Ik0tNTUwIDExODFsLTQgMjExIDM3NS0zNDZ6Ii8+CiAgPHBhdGggZmlsbD0iI2Y4ZjhmOCIgc3Ryb2tlPSIjZjhmOGY4IiBzdHJva2Utd2lkdGg9IjEuNTEiIGQ9Ik0tNTk4LTU2MWwyNDEgNjg5IDI0OC0xNTF6Ii8+CiAgPHBhdGggZmlsbD0iI2YyZjJmMiIgc3Ryb2tlPSIjZjJmMmYyIiBzdHJva2Utd2lkdGg9IjEuNTEiIGQ9Ik0tNDM1IDIwN2wxOTAgNTY1IDE0OS00NzV6Ii8+CiAgPHBhdGggZmlsbD0iI2Y0ZjRmNCIgc3Ryb2tlPSIjZjRmNGY0IiBzdHJva2Utd2lkdGg9IjEuNTEiIGQ9Ik0tMzU3IDEyOGwtNzggNzkgMzM5IDkweiIvPgogIDxwYXRoIGZpbGw9IiNlZWUiIHN0cm9rZT0iI2VlZSIgc3Ryb2tlLXdpZHRoPSIxLjUxIiBkPSJNLTI0NSA3NzJsLTMwNSA0MDkgMzcxLTEzNXoiLz4KICA8cGF0aCBmaWxsPSIjZjFmMWYxIiBzdHJva2U9IiNmMWYxZjEiIHN0cm9rZS13aWR0aD0iMS41MSIgZD0iTS00MzUgMjA3bC0xMTUgOTc0IDMwNS00MDl6Ii8+CiAgPHBhdGggZmlsbD0iI2Y4ZjhmOCIgc3Ryb2tlPSIjZjhmOGY4IiBzdHJva2Utd2lkdGg9IjEuNTEiIGQ9Ik0tNTk4LTU2MWwxNjMgNzY4IDc4LTc5eiIvPgogIDxwYXRoIGZpbGw9IiNmNWY1ZjUiIHN0cm9rZT0iI2Y1ZjVmNSIgc3Ryb2tlLXdpZHRoPSIxLjUxIiBkPSJNLTU5OC01NjFsNDggMTc0MiAxMTUtOTc0eiIvPgogIDxwYXRoIGZpbGw9IiNmMmYyZjIiIHN0cm9rZT0iI2YyZjJmMiIgc3Ryb2tlLXdpZHRoPSIxLjUxIiBkPSJNLTU5OC01NjFsNDQgMTk1MyA0LTIxMXoiLz4KPC9zdmc+Cg==);
	}

	form
	{
		grid-area: form;
	}

	.icon {
		&.emphasize {
			width: 24px !important;
			margin-left: 8px;
		}
	}

	.consent {
		&.displayName {
			display: flex;

		}
	}
</style>
