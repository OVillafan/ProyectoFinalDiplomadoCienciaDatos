# -*- coding: utf-8 -*-
"""
Created on Fri Apr  4 19:54:39 2025

@author: villa
"""

!pip install grpcio-tools

import os
from grpc_tools import protoc

input_file = r'C:\Users\villa\Documents\ProyectoFinal__DCD\Metrobus_GTFS_RT.proto'
output_file = r'C:\Users\villa\Documents\ProyectoFinal__DCD\Metrobus_GTFS_RT_limpio.proto'

with open(input_file, 'r', encoding='utf-8', errors='replace') as f:
    content = f.read()

# Reemplazar caracteres no ASCII y de control con espacio
cleaned = ''.join(c if 32 <= ord(c) < 127 else ' ' for c in content)
cleaned= 'syntax = "proto3";' + content;
# Guardar archivo limpio
with open(output_file, 'w', encoding='utf-8') as f:
    f.write(cleaned)

print("Archivo limpio guardado como:", output_file)

%
protoc.main([
    '',
    '-I', r'C:/Users/villa/Documents/ProyectoFinal__DCD',
    '--python_out=C:/Users/villa/Documents/ProyectoFinal__DCD',
    r'C:/Users/villa/Documents/ProyectoFinal__DCD/Metrobus_GTFS_RT_limpio.proto'
])