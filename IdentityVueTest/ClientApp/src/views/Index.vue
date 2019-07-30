<template>
	<div class = "content">
		<div class = "title md-display-2">
			Welcome<span v-if = "Model">, {{Model.UserName}}, </span>
										  to Mallenom Systems OIDC server
		</div>
		<md-list v-if = "Model">
			<md-subheader>Actions</md-subheader>
			<md-list-item :to = "{name: 'users'}"
						  v-if="Model.IsAdmin">Users</md-list-item>
			<md-list-item :to = "{name: 'apis'}"
						  v-if="Model.IsAdmin">APIs</md-list-item>
			<md-list-item :to = "{name: 'apps'}"
						  v-if="Model.IsAdmin">Applications</md-list-item>
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
	import Capitalize from "@/assets/ts/helpers/KeysCapitalizer";

	Vue.use(MdList);
	Vue.use(MdSubheader);

	@Component
	export default class Index
		extends Vue
	{
		public Model: IUserInfo | null = null;

		public async beforeMount()
		{
			try
			{
				const data: any = await Axios({
					method: "get",
					url:    "/api/v1/UserInfo",
				});

				const model = Capitalize(data.data);

				this.Model = model  as IUserInfo;
			}
			catch(exc)
			{
				this.Model = null;
			}
		}
	}
</script>

<style scoped
	   lang = "scss">
	.content {
		display: grid;

		grid-template-columns: 1fr 2fr 1fr 1fr 1fr 2fr 1fr;
		grid-template-rows: 1fr auto auto auto 1fr;

		width: 100vw;
		height: 100vh;
	}

	.title {
		grid-column-start: 2;
		grid-column-end: 6;

		grid-row-start: 2;

		margin-bottom: 8px;
	}

	.md-list {
		width: 320px;
		max-width: 100%;
		display: inline-block;
		vertical-align: top;
		border: 1px solid rgba(#000, .12);

		grid-column-start: 2;
		grid-row-start: 3;
		grid-row-end: 5;
	}
</style>
