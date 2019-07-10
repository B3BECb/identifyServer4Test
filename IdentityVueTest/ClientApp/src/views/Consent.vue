<template>
	<div>
		<md-content v-if = "Model"
					class = "md-elevation-3">
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
		<md-content v-else>
			Something went wrong
		</md-content>
	</div>
</template>

<script lang = "ts">
	import { Component, Vue } from "vue-property-decorator";
	import Axios from "axios";
	import { upperFirst } from "lodash";
	import {
		MdButton,
		MdContent,
		MdProgress,
		MdField,
		MdCheckbox,
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
	Vue.use(MdProgress);
	Vue.use(MdField);
	Vue.use(MdCheckbox);

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
	.title {
		display: flex;
		flex-direction: row;

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
	}
</style>
