<template>
	<div class="locale-changer">
		<select v-model="CurrentLang" @change="localeChanged()">
			<option v-for="(lang, i) in langs" :value="lang">{{ lang }}</option>
		</select>
	</div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import {AvailableLocales as locales} from "../i18n";

@Component
export default class LocaleChanger extends Vue {
	public CurrentLang = this.$i18n.locale;

	public langs = locales;

	public localeChanged() {
		this.$router.push({ name: this.$router.currentRoute.name, params: {locale: this.CurrentLang}});
	}
}
</script>