<template>
	<div class = "view login">
		<div></div>
		<div></div>
		<form @submit.prevent = "someAction()">
			<!--
			  Ошибка будет отображена пользователю сразу же,
			  флаг $v.passportData.$invalid говорит о том, валидное поле или нет
			-->
			<div>
				<input type = "text"
					   v-model = "passportData">
				<span v-if = "$v.name.$error">
					<span v-if = "$v.passportData.$invalid">
						Серия и номер паспорта должны быть в формате 1234 567890
				 	</span>
				</span>
			</div>

			<!--
			  Ошибка будет отображена после события blur

			  Метод $touch() выставит флагу $v.passportDate.$dirty значение true.
			  Флаг $v.passportDate.$error высчитывается как
			  $v.passportDate.$invalid && $v.passportDate.$dirty
			-->
			<!--<div>
				<input type = "text"
					   v-model = "passportDate"
					   @blur = "$v.passportDate.$touch()">
				<span v-if = "$v.passportDate.$error">
					Дата должна быть в формате ДД.ММ.ГГГГ
				  </span>
			</div>-->


			<!--
			  Поле, которое тоже выведет ошибку после события blur, но с другим подходом

			  $v.passportDate.$model - объект, при записи данных в который:
			  - Vuelidate присвоит переданное значение полю passportDate
			  - Vuelidate вызовет метод $touch() у объекта $v.passportDate

			  Модификатор lazy необходим, чтобы присваивание произошло только после blur
			-->
			<!--<div>
				<input type = "text"
					   v-model.lazy = "$v.passportDate.$model">
				<span v-if = "$v.passportDate.$error">
					Дата должна быть в формате ДД.ММ.ГГГГ
				  </span>
			</div>-->

			<!-- Поле с несколькими ошибками -->
			<div>
				<input type = "text"
					   v-model = "name"
					   @blur = "$v.name.$touch()">
				<span v-if = "$v.name.$error">
					<template v-if = "!$v.name.maxLength">
					  Длина имени не должна превышать {{ $v.name.$params.maxLength.max }} символов
					</template>
					<template v-else-if = "!$v.name.alpha">
					  Имя должно содержать только буквы
					</template>
					<template v-else>
					  Имя обязательно для заполнения
					</template>
				  </span>
			</div>

			<button type = "submit"
					:disabled = "$v.$invalid">
				Отправить форму
			</button>
		</form>
		<div></div>
		<div></div>
		<div>langs</div>
	</div>
</template>

<script lang = "ts">
	import { validationMixin, Validation } from "vuelidate";
	import {
		required,
		email,
		minLength,
		maxLength,
	} from "vuelidate/lib/validators";
	import { Component, Vue, Mixins } from "vue-property-decorator";

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
