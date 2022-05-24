public class Conta{
    public int id{get; set;}
     public bool lapide {get; set;}
    public string nome {get; set;}
    public string cpf {get; set;}
    public string cidade {get; set;}
    public int transferenciasRealizadas{get; set;}
    public float saldo {get; set;}

    public Conta(){

    }

    public Conta(string nome, string cpf, string cidade){
        id = 0;
        nome = nome;
        cpf = cpf;
        cidade = "";
        transferenciasRealizadas = 0;
        saldo = 0;
        lapide = false;
    }

    public void Deposito(float valor) {
        saldo += valor;
    }
    public void Transacao(float valor){
        transferenciasRealizadas++;
        saldo -= valor;
    }
    public void Deleta(){
        lapide = true;
    }

    public byte[] Serialize(){ using (memoryStream stream = new memoryStream()){
            using (BinaryWriter writer = new BinaryWriter(stream)){
                writer.Write(id);
                writer.Write(nome);
                writer.Write(cpf);
                writer.Write(cidade);
                writer.Write(transferenciasRealizadas);
                writer.Write(saldo);
            }
            return stream.ToArray();
        }

    }

    public void Deserializer(byte[] buffer){ using (MemoryStream stream = new MemoryStream(buffer)){            
            using (BinaryReader reader = new BinaryReader(stream)){
                id = reader.ReadUInt16();
                nome = reader.ReadString();
                cpf = reader.ReadString();
                cidade = reader.ReadString();
                transferenciasRealizadas = reader.ReadUInt16();
                saldo =  reader.ReadSingle();
            }
        }
    }

    public override string ToString(){
        return $"id: {id}\n Nome:{nome}\n CPF: {cpf}\n Cidade: {cidade}\n"+
        $"Transferencias Realizadas: {transferenciasRealizadas}\n Saldo: {saldo}";
    }

}