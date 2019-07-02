import Vue from "vue";
import App from "./App.vue";
import router from "./router";
import "./registerServiceWorker";
import store from "./store";
import i18n from "./i18n";
import "reset.css";
import "typeface-roboto";

Vue.config.productionTip = false;

new Vue({
  router,
  store,
  i18n,
  render: (h) => h(App),
}).$mount("#app");