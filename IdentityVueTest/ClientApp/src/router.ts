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
			path: "/account/logout/prompt",
			redirect: `/${I18n.default.locale}/account/logout`,
		},
		{
			path: "/:locale/account/logout",
			name:      "logout",
			component: () => import(/* webpackChunkName: "logout" */ "./views/Logout.vue"),
		},
		{
			path: "/account/loggedOut",
			redirect: `/${I18n.default.locale}/account/loggedOut`,
		},
		{
			path: "/:locale/account/loggedOut",
			name:      "loggedOut",
			component: () => import(/* webpackChunkName: "loggedOut" */ "./views/LoggedOut.vue"),
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
	],
});

router.beforeEach((to, from, next) => {
	const lang = to.params.locale;
	I18n.loadLanguageAsync(lang).then(() => next());
	Vue.cookies.set("lang", lang, "1Y");
});

export default router;
