<template>
	<form class = "md-layout"
		  @submit = "submit"
		  action = "/api/v1/consent"
		  method = "post">
		<md-card class = "md-layout-item md-size-60 md-small-size-100"
				 v-if = "Model">
			<md-card-header>
				<div>
					<span class = "name md-title">{{Model.ClientName}}</span>
					<span class = "md-subheading"> is requesting your permission</span>
				</div>
			</md-card-header>
			<md-card-content>
				<div class = "md-layout md-gutter md-alignment-center-left">
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
	</form>
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
