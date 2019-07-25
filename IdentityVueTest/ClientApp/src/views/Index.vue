<template>
	<div class = "content">
		<div class = "title md-display-2">
			Welcome<span v-if = "UserName">, {{UserName}}, </span>
										  to Mallenom Systems OIDC server
		</div>
		<md-list v-if = "UserName">
			<md-subheader>Actions</md-subheader>
			<md-list-item :to = "{name: 'grants'}">Stored grants</md-list-item>
			<md-list-item :to = "{name: 'logout'}">Logout</md-list-item>
		</md-list>
		<md-list v-else>
			<md-subheader>Actions</md-subheader>
			<md-list-item href = "/account/login">Login</md-list-item>
		</md-list>
	</div>
</template>

<script lang = "ts">
	import { Component, Vue } from "vue-property-decorator";
	import {
		MdList,
		MdSubheader,
		// @ts-ignore
	} from "vue-material/dist/components";
	import Axios from "axios";

	Vue.use(MdList);
	Vue.use(MdSubheader);

	@Component
	export default class Index
		extends Vue
	{
		public UserName: string | null = null;

		public async beforeMount()
		{
			try
			{
				let data: any = await Axios({
					method: "get",
					url:    "/api/v1/UserInfo",
				});

				this.UserName = data.data.userName;
			}
			catch(exc)
			{
				this.UserName = null;
			}
		}
	};
</script>

<style scoped
	   lang = "scss">
	.content {
		display: grid;

		grid-template-columns: 1fr 2fr 1fr 1fr 1fr 2fr 1fr;
		grid-template-rows: 1fr 1fr 1fr 1fr 1fr;

		width: 100vw;
		height: 100vh;
	}

	.title {
		grid-column-start: 2;
		grid-column-end: 6;

		grid-row-start: 2;
	}

	.md-list {
		width: 320px;
		max-width: 100%;
		display: inline-block;
		vertical-align: top;
		border: 1px solid rgba(#000, .12);

		grid-column-start: 2;
		grid-row-start: 3;
	}
</style>
