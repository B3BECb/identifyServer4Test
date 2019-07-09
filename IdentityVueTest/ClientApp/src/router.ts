import Vue from "vue";
import Router from "vue-router";
import * as I18n from "./i18n";

Vue.use(Router);

const router = new Router({
	mode:   "history",
	base:   process.env.BASE_URL,
	routes: [
		{
			path: "/",
			redirect: `/${I18n.default.locale}`,
		},
		{
			path: "/account/login",
			redirect: `/${I18n.default.locale}/account/login`,
		},
		{
			path: "/:locale/account/login",
			name:      "login",
			component: () => import(/* webpackChunkName: "login" */ "./views/Login.vue"),
		},
		{
			path: "/grants",
			redirect: `/${I18n.default.locale}/grants`,
		},
		{
			path: "/:locale/grants",
			name:      "grants",
			component: () => import(/* webpackChunkName: "grants" */ "./views/Grants.vue"),
		},
		{
			path: "/consent",
			redirect: `/${I18n.default.locale}/consent`,
		},
		{
			path: "/:locale/consent",
			name:      "consent",
			component: () => import(/* webpackChunkName: "consent" */ "./views/Consent.vue"),
		},
		// {
		// 	path: "/:locale",
		// 	name:      "home",
		// 	component: Home,
		// },
		// {
		// 	path:      "/:locale/about",
		// 	name:      "about",
		// 	component: () => import(/* webpackChunkName: "about" */ "./views/About.vue"),
		// },
	],
});

router.beforeEach((to, from, next) => {
	const lang = to.params.locale;
	I18n.loadLanguageAsync(lang).then(() => next());
});

export default router;
