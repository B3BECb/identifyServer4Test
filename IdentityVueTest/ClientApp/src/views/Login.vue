<template>
	<div class = "view login">
		<div></div>
		<div></div>
		<form @submit.prevent = "someAction()">
			<md-card class = "md-layout-item md-size-50 md-small-size-100">
				<md-card-header>
					<div class = "md-title">Users</div>
				</md-card-header>

				<md-card-content>

				</md-card-content>

				<md-progress-bar md-mode = "indeterminate"
								 v-if = "sending" />

				<md-card-actions>
					<md-button type = "submit"
							   class = "md-primary"
							   :disabled = "$v.$invalid">Create user
					</md-button>
				</md-card-actions>
			</md-card>
		</form>
		<div></div>
		<div></div>
		<div>langs</div>
	</div>
</template>

<script lang = "ts">
	import { validationMixin } from "vuelidate";
	import {
		required,
		email,
		minLength,
		maxLength,
	} from "vuelidate/lib/validators";
	import { Component, Vue } from "vue-property-decorator";
	import { MdButton, MdContent, MdCard, MdProgress } from 'vue-material/dist/components';

	Vue.use(MdButton);
	Vue.use(MdContent);
	Vue.use(MdCard);
	Vue.use(MdProgress);

	@Component({
		mixins:      [validationMixin],
		validations: {
			passportData: {
				required,
				validFormat: (val: string) => /^\d{4} \d{6}$/.test(val),
			},
			name:         {
				required,
				maxLength: maxLength(10),
				alpha:     (val: string) => /^[а-яё]*$/i.test(val),
			},
		},
	})
	export default class Login
		extends Vue
	{
		passportData = null;
		name         = null;
		sending      = true;
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
