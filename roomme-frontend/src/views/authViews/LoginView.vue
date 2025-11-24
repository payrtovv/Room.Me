<script setup>
import { RouterLink } from 'vue-router';


const handleRegister = () => {
  console.log('Registrando...');
  // todo
}

//-----states------
const email = ref('')
const password = ref('')

//-----validation Rules-------
const rules = reactive({
    required: (value)=> !!value ? true : 'Este campo es requerido',
})


</script>


<template>

    <p class="text-h4 mb-4 font-weight-black">¡Bienvenido!</p>
    <p class=" text-h6 font-weight-light mb-10">Qué gusto verte. Tu roomie ideal te está esperando.</p>

  <v-form @submit.prevent="handleRegister">
    
    <p class="text-black mb-2">Correo Electrónico</p>
    <v-text-field
      type="email"
      v-model="email"
      variant="outlined"
      :rules="[rules.required]"
    ></v-text-field>

    <p class="text-black mt-4 mb-2">Contraseña</p>
    <v-text-field
      type="password"
      v-model="password"
      variant="outlined"
      :rules="[rules.required]"
    ></v-text-field>

    <p class="mb-10" style="cursor: pointer;">¿Olvidaste tu contraseña?</p>

    <v-btn
      type="submit"
      color="primary"
      block
      size="large"
      rounded
      class="text-none"
    >
      Iniciar Sesión
    </v-btn>

    <p class="text-h6 text-center mt-6">¿No tienes una cuenta? 
      <RouterLink 
        class="text-primary text-decoration-none"
        :to="{name:'register'}"
      >
        Regístrate aquí
      </RouterLink>
    </p>

  </v-form>
</template>

