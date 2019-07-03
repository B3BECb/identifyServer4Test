<template>
	<div class = "view login">
		<div></div>
		<div></div>
		<form @submit.prevent = "someAction()">
			<md-card class="md-layout-item md-size-50 md-small-size-100">
				<md-card-header>
					<div class="md-title">Users</div>
				</md-card-header>
				<md-button type="submit" class="md-primary" :disabled="$v.$invalid">Create user</md-button>
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
	import { MdButton, MdContent, MdCard } from 'vue-material/dist/components'

	Vue.use(MdButton)
	Vue.use(MdContent)
	Vue.use(MdCard)

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
