<template>
	<div>
		<form class="login" @submit.prevent="login">
			<h1>Sign in</h1>
			<label>User name</label>
			<input required v-model="username" type="text" placeholder="Snoopy"/>
			<label>Password</label>
			<input required v-model="password" type="password" placeholder="Password"/>
			<hr/>
			<button type="submit">Login</button>
		</form>
	</div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";

@Component
export default class Authorization extends Vue {
	// public async login() {
	// 	const { username, password } = this;
	// 	await this.$store.dispatch(AUTH_REQUEST, { username, password });
	// 	this.$router.push("/");
	// }

	// public LoginRoutine(user) {
	//
	// }
}
</script>

<style scoped>

</style>
