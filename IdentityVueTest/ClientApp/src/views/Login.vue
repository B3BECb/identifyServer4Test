<template>
	<div>
		<div class = "md-layout md-alignment-center">
			<div></div>
			<div></div>
			<md-card class = "md-layout-item md-size-100 md-small-size-50">
				<form @submit.prevent = "someAction()">
					<md-card-header>
						<div class = "md-title">Mallenom systems</div>
					</md-card-header>

					<md-card-content>
						<md-field :class = "getValidationClass('login')">
							<label>{{$t('login')}}</label>
							<md-input v-model = "login"></md-input>
							<span class = "md-error"
								  v-if = "!$v.login.required">{{$t('loginError')}}</span>
						</md-field>
					</md-card-content>

					<md-card-content>
						<md-field :class = "getValidationClass('password')">
							<label>{{$t('password')}}</label>
							<md-input v-model = "password"></md-input>
							<span class = "md-error"
								  v-if = "!$v.password.required">{{$t('passwordError')}}</span>
						</md-field>
					</md-card-content>

					<md-progress-bar md-mode = "indeterminate"
									 v-if = "sending" />

					<md-card-actions>
						<md-button type = "submit"
								   class = "md-primary"
								   :disabled = "sending">Create user
						</md-button>
					</md-card-actions>
				</form>
			</md-card>
			<div></div>
			<div></div>
		</div>
		<div class="md-bottom-bar">langs</div>
	</div>
</template>

<script lang = "ts">
	import { validationMixin } from "vuelidate";
	import {
		required,
	} from "vuelidate/lib/validators";
	import { Component, Vue } from "vue-property-decorator";
	// @ts-ignore
	import { MdButton, MdContent, MdCard, MdProgress, MdField } from 'vue-material/dist/components';

	Vue.use(MdButton);
	Vue.use(MdContent);
	Vue.use(MdCard);
	Vue.use(MdProgress);
	Vue.use(MdField);

	@Component({
		mixins:      [validationMixin],
		validations: {
			login:    {
				required,
			},
			password: {},
		},
	})
	export default class Login
		extends Vue
	{
		login    = null;
		password = null;
		sending  = false;

		getValidationClass(fieldName)
		{
			let field = this.$v[fieldName];

			if(field)
			{
				return {
					'md-invalid': field.$invalid && field.$dirty,
				};
			}
		}

		someAction()
		{
			this.$v.$touch();
		}
	}
</script>

<style scoped
	   lang = "scss">
	.md-progress-bar {
		position: absolute;
		top: 0;
		right: 0;
		left: 0;
	}
</style>
