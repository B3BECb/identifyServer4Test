<template>
	<form class = "md-layout"
		  @submit = "submit"
		  action = "/api/v1/consent"
		  method = "post">
		<md-card class = "md-layout-item md-size-50 md-small-size-100"
				 v-if = "Model">
			<md-card-header>
				<div>
					<div class = "name md-title">{{Model.ClientName}}</div>
					<div class="md-subheading">is requesting your permission</div>
				</div>
			</md-card-header>
			<md-card-content>
				<div class = "md-layout md-gutter content">
					<div class = "md-layout-item md-small-size-100">
						<input type = "hidden"
							   v-model = "XSRF"
							   name = "XSRF-TOKEN-FIELD" />
					</div>
					<div class = "md-layout-item md-small-size-100">
						<input type = "hidden"
							   v-model = "ReturnUrl"
							   name = "returnUrl" />
					</div>
					<div class = "md-layout-item md-small-size-100"
						 v-if = "Model.IdentityScopes.length">
						<div class = "panel-heading">
							<md-icon class = "icon">account_circle</md-icon>
							Personal Information
						</div>
						<ul class = "list-group">
							<li v-for = "scope in Model.IdentityScopes"
								class = "list-group-item">
								<label>
									<input class = "consent-scopecheck"
										   type = "checkbox"
										   name = "ScopesConsented"
										   v-bind:id = "'scopes_' + scope.Name"
										   v-bind:value = "scope.Name"
										   v-bind:checked = "scope.Checked"
										   v-bind:disabled = "scope.Required" />
									<input v-if = "scope.Required"
										   type = "hidden"
										   name = "ScopesConsented"
										   v-bind:value = "scope.Name" />
									<strong>{{scope.DisplayName}}</strong>
									<md-icon class = "icon"
											 v-if = "scope.Emphasize">error
									</md-icon>
								</label>
								<span v-if = "scope.Required"><em>(required)</em></span>
								<div v-if = "scope.Description"
									 class = "consent-description">
									<label v-bind:for = "'scopes' + scope.Name">{{scope.Description}}</label>
								</div>
							</li>
						</ul>
					</div>
					<div class = "md-layout-item md-small-size-100"
						 v-if = "Model.ResourceScopes.length">
						<div class = "panel-heading">
							<md-icon class = "icon">list</md-icon>
							Application Access
						</div>
						<ul class = "list-group">
							<li v-for = "scope in Model.ResourceScopes"
								class = "list-group-item">
								<label>
									<input class = "consent-scopecheck"
										   type = "checkbox"
										   name = "ScopesConsented"
										   v-bind:id = "'scopes_' + scope.Name"
										   v-bind:value = "scope.Name"
										   v-bind:checked = "scope.Checked"
										   v-bind:disabled = "scope.Required" />
									<input v-if = "scope.Required"
										   type = "hidden"
										   name = "ScopesConsented"
										   v-bind:value = "scope.Name" />
									<strong>{{scope.DisplayName}}</strong>
									<span v-if = "scope.Emphasize"
										  class = "glyphicon glyphicon-exclamation-sign"></span>
								</label>
								<span v-if = "scope.Required"><em>(required)</em></span>
								<div v-if = "scope.Description"
									 class = "consent-description">
									<label v-bind:for = "'scopes' + scope.Name">{{scope.Description}}</label>
								</div>
							</li>
						</ul>
					</div>
				</div>
			</md-card-content>
			<md-card-actions>
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
			</md-card-actions>
		</md-card>
		<!--<md-content v-if = "Model"
					class = "md-layout-item md-elevation-3 md-size-50">
			<div class = "title">
				<div v-if = "Model.ClientLogoUrl"
					 class = "logo"><img v-bind:src = "Model.ClientLogoUrl"></div>
				<div class = "appInfo">
					<div class = "name">{{Model.ClientName}}</div>
					<div>is requesting your permission</div>
				</div>
			</div>

			<div class = "row">
				<div class = "col-sm-8">

					<form @submit = "submit"
						  action = "/api/v1/consent"
						  method = "post">
						<input type = "hidden"
							   v-model = "XSRF"
							   name = "XSRF-TOKEN-FIELD" />
						<input type = "hidden"
							   v-model = "ReturnUrl"
							   name = "returnUrl" />

						<div>Uncheck the permissions you do not wish to grant.</div>

						<div v-if = "Model.IdentityScopes.length"
							 class = "panel panel-default consent-buttons">
							<div class = "panel-heading">
								<span class = "glyphicon glyphicon-user"></span>
								Personal Information
							</div>
							<ul class = "list-group">
								<li v-for = "scope in Model.IdentityScopes"
									class = "list-group-item">
									<label>
										<input class = "consent-scopecheck"
											   type = "checkbox"
											   name = "ScopesConsented"
											   v-bind:id = "'scopes_' + scope.Name"
											   v-bind:value = "scope.Name"
											   v-bind:checked = "scope.Checked"
											   v-bind:disabled = "scope.Required" />
										<input v-if = "scope.Required"
											   type = "hidden"
											   name = "ScopesConsented"
											   v-bind:value = "scope.Name" />
										<strong>{{scope.DisplayName}}</strong>
										<span v-if = "scope.Emphasize"
											  class = "glyphicon glyphicon-exclamation-sign"></span>
									</label>
									<span v-if = "scope.Required"><em>(required)</em></span>
									<div v-if = "scope.Description"
										 class = "consent-description">
										<label v-bind:for = "'scopes' + scope.Name">{{scope.Description}}</label>
									</div>
								</li>
							</ul>
						</div>

						<div v-if = "Model.ResourceScopes.length"
							 class = "panel panel-default">
							<div class = "panel-heading">
								<span class = "glyphicon glyphicon-tasks"></span>
								Application Access
							</div>
							<ul class = "list-group">
								<li v-for = "scope in Model.ResourceScopes"
									class = "list-group-item">
									<label>
										<input class = "consent-scopecheck"
											   type = "checkbox"
											   name = "ScopesConsented"
											   v-bind:id = "'scopes_' + scope.Name"
											   v-bind:value = "scope.Name"
											   v-bind:checked = "scope.Checked"
											   v-bind:disabled = "scope.Required" />
										<input v-if = "scope.Required"
											   type = "hidden"
											   name = "ScopesConsented"
											   v-bind:value = "scope.Name" />
										<strong>{{scope.DisplayName}}</strong>
										<span v-if = "scope.Emphasize"
											  class = "glyphicon glyphicon-exclamation-sign"></span>
									</label>
									<span v-if = "scope.Required"><em>(required)</em></span>
									<div v-if = "scope.Description"
										 class = "consent-description">
										<label v-bind:for = "'scopes' + scope.Name">{{scope.Description}}</label>
									</div>
								</li>
							</ul>
						</div>

						<div v-if = "Model.AllowRememberConsent"
							 class = "consent-remember">
							<label>
								<input class = "consent-scopecheck"
									   name = "RememberConsent" />
								<strong>Remember My Decision</strong>
							</label>
						</div>

						<div class = "consent-buttons">
							<button name = "button"
									value = "yes"
									class = "btn btn-primary"
									autofocus>Yes, Allow
							</button>
							<button name = "button"
									value = "no"
									class = "btn">No, Do Not Allow
							</button>
							<a v-if = "Model.ClientUrl"
							   class = "pull-right btn btn-default"
							   target = "_blank"
							   @href = "Model.ClientUrl">
								<span class = "glyphicon glyphicon-info-sign"></span>
								<strong>{{Model.ClientName}}</strong>
							</a>
						</div>
					</form>
				</div>
			</div>

		</md-content>
		<md-content v-else
					class = "md-layout-item">
			Something went wrong
		</md-content>-->
	</form>
</template>

<script lang = "ts">
	import { Component, Vue } from "vue-property-decorator";
	import Axios from "axios";
	import { upperFirst } from "lodash";
	import {
		MdButton,
		MdContent,
		MdCard,
		MdProgress,
		MdField,
		MdCheckbox,
		MdIcon,
		// @ts-ignore
	} from "vue-material/dist/components";

	interface IConsentInputModel
	{
		Button: string;

		ScopesConsented: string[];

		RememberConsent: boolean;

		ReturnUrl: string;
	}

	interface IConsentViewModel
		extends IConsentInputModel
	{
		ClientName: string;

		ClientUrl: string;

		ClientLogoUrl: string;

		AllowRememberConsent: boolean;

		IdentityScopes: IScopeViewModel[];

		ResourceScopes: IScopeViewModel[];
	}

	interface IScopeViewModel
	{
		Name: string;

		DisplayName: string;

		Description: string;

		Emphasize: boolean;

		Required: boolean;

		Checked: boolean;
	}

	Vue.use(MdButton);
	Vue.use(MdContent);
	Vue.use(MdCard);
	Vue.use(MdProgress);
	Vue.use(MdField);
	Vue.use(MdCheckbox);
	Vue.use(MdIcon);

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

			data = this.CapitalizeKeys(data.data);

			console.log(data);

			this.Model = data;

			console.log(this.Model);
		}

		public submit(e: Event)
		{
			this.ReturnUrl = (this.$route.query as any).ReturnUrl || "";

			this.XSRF = this.$cookies.get("XSRF-TOKEN");

			return true;
		}

		private CapitalizeKeys(obj: any): any
		{
			if(Array.isArray(obj))
			{
				return obj.map((v: any) => this.CapitalizeKeys(v));
			}
			else
			{
				if(obj !== null && obj.constructor === Object)
				{
					return Object.keys(obj).reduce(
						(result, key) => ({
							...result,
							[upperFirst(key)]: this.CapitalizeKeys(obj[key]),
						}),
						{},
					);
				}
			}
			return obj;
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

	.icon {
		font-family: "Material Icons", sans-serif !important;
	}

	.appInfo {
		display: flex;
		flex-direction: row;
		align-items: baseline;

		.name {
			font-size: 2em;
			font-weight: bold;
			margin-right: 8px;
		}
	}

	.content {
		flex-direction: column;
	}
</style>
