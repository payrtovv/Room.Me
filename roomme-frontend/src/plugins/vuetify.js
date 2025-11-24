
import '@mdi/font/css/materialdesignicons.css'
import 'vuetify/styles'

import { createVuetify } from 'vuetify'

const lightPalette = {
  dark: false,
  colors: {
    // --- Mapeo de tu nueva paleta ---
    'background': '#D6EBF7',   // Fondo de Página
    'surface': '#F8F8FF',      // Fondo de Contenedor
    'primary': '#46A8DA',      // Botón Primario (Fondo)
    'secondary': '#207BBF',    // Texto Secundario / Botón Hover
    'accent': '#73C2FB',       // Links y Acentos

    // --- Colores de TEXTO (on-*) ---
    // Vuetify los usa para saber qué color de texto poner ENCIMA de cada fondo
    'on-background': '#00509D', // Texto Principal (sobre el fondo de página)
    'on-surface': '#00509D',    // Texto Principal (sobre los contenedores)
    'on-primary': '#F8F8FF',    // Botón Primario (Texto) (sobre el botón primario)

    // --- Colores de estado (Mantenemos los originales que ya tenías) ---
    'error': '#B71C1C',
    'info': '#0288D1',
    'success': '#388E3C',
    'warning': '#F57C00',
  }
}

export default createVuetify({
  defaultTheme: 'light', // Empieza con el tema ligero
    themes: {
      light: lightPalette, // Registra tu paleta
    },
  defaults:{
    VMessages:{
      transition:{
        name: 'v-scale-transition',
        mode: 'out-in'
      } 
      
    }
  }
})
