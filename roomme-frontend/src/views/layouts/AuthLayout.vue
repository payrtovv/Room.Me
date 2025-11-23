<template>
    <v-container fluid class="pa-0 h-100 vh-100" >
        <v-row no-gutters class="h-100">
            <!--Image column-->
            <v-col 
                cols="12" 
                md="7"
                class="position-relative d-flex align-center justify-center"    
            >
                <v-img 
                    src="@/assets/images/loginLayout.png"
                    cover
                    class="fill-height"
                    style="position:absolute; top:0; left:0; width: 100%; height: 100%;"    
                />
                <h1 class="room-me-text">room.me</h1>
                    
            </v-col>
            <v-col cols="12" md="5" class="h-100 d-flex align-center justify-center">
            <v-sheet class="ma-8">
                <RouterView />
            </v-sheet>
            </v-col>
        </v-row>
    </v-container>
</template>

<style scoped>

.room-me-text{
    font-size: 4rem;
    font-weight: 700;
    z-index: 1;
    position: relative;
}

</style>