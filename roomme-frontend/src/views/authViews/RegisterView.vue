<script setup>
import { ref, reactive } from 'vue';
import { RouterLink } from 'vue-router';

const handleRegister = ()=> {
  console.log('Registrando...');
  // Aquí irá la lógica de registro
}

//-----states------
const name = ref('')
const lastName = ref('')
const email = ref('')
const password = ref('')


//-----validation Rules-------

const rules = reactive({
    required: (value)=> !!value ? true : 'Este campo es requerido',
    passwordMatches: (value)=> value === password.value ? true : 'Las contraseñas no coinciden'
})


</script>
<template>

    <p class="text-h4 mb-4 font-weight-black">Regístrate</p>
    <p class=" text-h6 font-weight-light mb-10">Regístrate gratis y encuentra al roomie perfecto para ti.</p>

  <v-form @submit.prevent="handleRegister">
    
    <v-row>
        <v-col cols="12" md="6">
            <p class="text-black mb-2">Nombre</p>
            <v-text-field
                type="text"
                v-model="name"
                variant="outlined"
                :rules="[rules.required]"
            >
                
            </v-text-field>
        </v-col>

        <v-col cols="12" md="6">
            <p class="text-black mb-2">Apellido</p>
            <v-text-field
                type="text"
                v-model="lastName"
                variant="outlined"
                :rules="[rules.required]"
            >
                
            </v-text-field>
        </v-col>
    </v-row>

    <p class="text-black mt-4 mb-2">Correo Electrónico</p>
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

    <p class="text-black mt-4 mb-2">Confirmar Contraseña</p>
    <v-text-field
      type="password"
      variant="outlined"
      :rules="[rules.required, rules.passwordMatches]"
      validate-on="blur"
    ></v-text-field>

    <v-btn
      type="submit"
      color="primary"
      block
      size="large"
      rounded
      class="text-none mt-4"
    >
      Registrarse
    </v-btn>

    <p class="text-h6 text-center mt-6">¿Ya tienes una cuenta? 
      <RouterLink 
        class="text-primary text-decoration-none"
        :to="{name:'login'}"
      >
        Inicia sesión
      </RouterLink>
    </p>

  </v-form>
</template>