import Vue from "vue";
import VueI18n, { LocaleMessages } from "vue-i18n";
import axios from "axios";
import VueCookies from "vue-cookies";

Vue.use(VueCookies);
Vue.use(VueI18n);

const DEFAULT_LANG = "en";

const loadedLanguages = [DEFAULT_LANG];

const cookieLocale = Vue.cookies.get("lang");

const AvailableLocales = getAvailableLocales();

function getAvailableLocales(): string[] {
	const locales = require.context("./locales", true, /[A-Za-z0-9-_,\s]+\.json$/i);
	return locales.keys().map((key) => {
		const matched = key.match(/([A-Za-z0-9-_]+)\./i);
		if (matched && matched.length > 1) {
			return matched[1];
		} else {
			return key;
		}
	});
}

function loadLocale(lang: string): LocaleMessages {
	const locale                   = require(`./locales/${ lang }.json`);
	const messages: LocaleMessages = {};
	messages[lang]                 = locale;
	setLangParams(lang);
	return messages;
}

const i18n = new VueI18n({
	locale:         cookieLocale || process.env.VUE_APP_I18N_LOCALE || DEFAULT_LANG,
	fallbackLocale: process.env.VUE_APP_I18N_FALLBACK_LOCALE || DEFAULT_LANG,
	messages:       loadLocale(cookieLocale || process.env.VUE_APP_I18N_LOCALE || DEFAULT_LANG),
});

function setI18nLanguage(lang: string) {
	i18n.locale = lang;
	setLangParams(lang);
	return lang;
}

function setLangParams(lang: string) {
	axios.defaults.headers.common["Accept-Language"] = lang;
	const html: HTMLElement                            = document.querySelector("html") as HTMLElement;
	html.setAttribute("lang", lang);
}

export function loadLanguageAsync(lang: string) {
	if (i18n.locale !== lang) {
		if (!loadedLanguages.includes(lang)) {
			return import(/* webpackChunkName: "lang-[request]" */ `@/locales/${ lang }`)
				.then((msgs) => {
					i18n.setLocaleMessage(lang, msgs.default);
					loadedLanguages.push(lang);
					return setI18nLanguage(lang);
				})
				.catch((exc) => {
					console.warn(`Unable to load locale (${ lang }) resolved default locale (${ DEFAULT_LANG })`);
					return Promise.resolve(DEFAULT_LANG);
				});
		}
		return Promise.resolve(setI18nLanguage(lang));
	}
	return Promise.resolve(lang);
}

export default i18n;
export {
	DEFAULT_LANG as defaultLocale,
	AvailableLocales,
};
