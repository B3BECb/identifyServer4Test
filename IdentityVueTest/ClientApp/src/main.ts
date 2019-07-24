import Vue from "vue";
import App from "./App.vue";
import VueCookies from "vue-cookies";
import router from "./router";
import "./registerServiceWorker";
import store from "./store";
import i18n from "./i18n";
import Vuelidate from "vuelidate";

Vue.config.productionTip = false;
Vue.use(Vuelidate);
Vue.use(VueCookies);

new Vue({
  router,
  store,
  i18n,
  render: (h) => h(App),
}).$mount("#app");

