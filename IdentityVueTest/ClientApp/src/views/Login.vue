<template>
	<div class="view login">
		<div></div>
		<div></div>
		<form novalidate class="md-layout" @submit.prevent="validateUser">
			<md-card class="md-layout-item md-size-50 md-small-size-100">
				<md-card-header>
					<div class="md-title">Users</div>
				</md-card-header>

				<md-card-content>
					<div class="md-layout md-gutter">
						<div class="md-layout-item md-small-size-100">
							<md-field :class="getValidationClass('firstName')">
								<label for="first-name">First Name</label>
								<md-input name="first-name" id="first-name" autocomplete="given-name" v-model="form.firstName" :disabled="sending" />
								<span class="md-error" v-if="!$v.form.firstName.required">The first name is required</span>
								<span class="md-error" v-else-if="!$v.form.firstName.minlength">Invalid first name</span>
							</md-field>
						</div>

						<div class="md-layout-item md-small-size-100">
							<md-field :class="getValidationClass('lastName')">
								<label for="last-name">Last Name</label>
								<md-input name="last-name" id="last-name" autocomplete="family-name" v-model="form.lastName" :disabled="sending" />
								<span class="md-error" v-if="!$v.form.lastName.required">The last name is required</span>
								<span class="md-error" v-else-if="!$v.form.lastName.minlength">Invalid last name</span>
							</md-field>
						</div>
					</div>

					<md-field :class="getValidationClass('email')">
						<label for="email">Email</label>
						<md-input type="email" name="email" id="email" autocomplete="email" v-model="form.email" :disabled="sending" />
						<span class="md-error" v-if="!$v.form.email.required">The email is required</span>
						<span class="md-error" v-else-if="!$v.form.email.email">Invalid email</span>
					</md-field>
				</md-card-content>

				<md-progress-bar md-mode="indeterminate" v-if="sending" />

				<md-card-actions>
					<md-button type="submit" class="md-primary" :disabled="sending">Create user</md-button>
				</md-card-actions>
			</md-card>

			<md-snackbar :md-active.sync="userSaved">The user {{ lastUser }} was saved with success!</md-snackbar>
		</form>
		<div class="form login">
			<div>Mallenom</div>
			<div>email/login</div>
			<div>password</div>
			<div>Sing in</div>
			<div>providers</div>
		</div>
		<div></div>
		<div></div>
		<div>langs</div>
	</div>
</template>

<script lang="ts">
	import { validationMixin, Validation } from "vuelidate";
	import {
		required,
		email,
		minLength,
		maxLength,
	} from "vuelidate/lib/validators";
	import { Component, Vue, Mixins } from "vue-property-decorator";

	// @Component
	export default class Login extends Mixins<Validation>(validationMixin) {
		public form = {
			firstName: null,
			lastName: null,
			age: null,
			email: null,
		};
		public userSaved = false;
		public sending = false;
		public lastUser = null;
		public validations = {
			form: {
				firstName: {
					required,
					minLength: minLength(3),
				},
				lastName: {
					required,
					minLength: minLength(3),
				},
				age: {
					required,
					maxLength: maxLength(3),
				},
				email: {
					required,
					email,
				},
			},
		};
		getValidationClass (fieldName)
		{
			const field = this.$v.form[fieldName];

			if (field) {
				return {
					'md-invalid': field.$invalid && field.$dirty
				}
			}
		}
	}
</script>

<style scoped lang="scss">
	.md-progress-bar {
		position: absolute;
		top: 0;
		right: 0;
		left: 0;
	}
</style>
