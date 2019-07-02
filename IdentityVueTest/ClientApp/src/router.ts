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
			path: "/Account/Login",
			redirect: `/${I18n.default.locale}/Account/Login`,
		},
		{
			path: "/:locale/Account/Login",
			name:      "login",
			component: () => import(/* webpackChunkName: "login" */ "./views/Login.vue"),
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
