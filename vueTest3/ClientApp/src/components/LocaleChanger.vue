<template>
	<div class="locale-changer">
		<select v-model="CurrentLang" @change="localeChanged()">
			<option v-for="(lang, i) in langs" :value="lang">{{ lang }}</option>
		</select>
	</div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import * as i18nModule from "../i18n";

@Component
export default class LocaleChanger extends Vue {
	private _currentLang;

	public langs = i18nModule.AvailableLocales;

	public localeChanged() {
		this.$router.push({ name: this.$router.currentRoute.name, params: {locale: this._currentLang}});
	}

	public get CurrentLang()
	{
		return i18nModule.default.locale;
	}

	public set CurrentLang(value)
	{
		this._currentLang = value;
	}
}
</script>
