public class FileManager{
    private string contaDb = "C:\\TP2_Aeds3\\Conta.db"; 
    private string indexDb = "C:\\TP2_Aeds3\\Index.db";
    private string invertedCity = "C:\\TP2_Aeds3\\City.db";
    private string invertedName = "C:\\TP2_Aeds3\\Name.db";

    public bool Create(ContaBancaria conta){
        long position;
        if(conta = null){return false;}
        using (FileStream file = new FileStream(contaDb, FileMode.Open, FileAccess.Read)){
            using(BinaryReader file = new BinaryReader(file)){
                file.BaseStream.Seek(0, SeekOrigin.Begin);
                conta.id = (int)(file.ReadUInt16() + 1);
            }
            somaMaxId(conta.id);
        }
		using (FileStream file = new FileStream(contaDB, FileMode.Open, FileAccess.Write)) {
            using (BinaryWriter writer = new BinaryWriter(file)) {
                writer.Seek(0, SeekOrigin.End);
                position = writer.BaseStream.Position;
                writer.Write(false);
                writer.Write(conta.Serialize().Length);
                writer.Write(conta.Serialize());
            }
        }
        CreateIndex(conta.id, position);
        CreateInvertedFiles();
        return true;
    }

    public void BuildFile() {

        BuildIndexFile();

        BuildInvertedFiles();

        if (!File.Exists(contaDb)) {
            using (FileStream file = new FileStream(contaDb, FileMode.Create)) {
                using (BinaryWriter writer = new BinaryWriter(file)) {
                    writer.Seek(0, SeekOrigin.Begin);
                    writer.Write((int)0);
                }
            }
        } else {
            using (FileStream stream = new FileStream(contaDb, FileMode.Open, FileAccess.ReadWrite)) {
                using (BinaryReader reader = new BinaryReader(stream)) {
                    reader.BaseStream.Seek(0, SeekOrigin.Begin);
                    long length = stream.Length;
                    if (length == 0) {
                        using (BinaryWriter writer = new BinaryWriter(stream)) {
                            writer.Write((int)0);
                        }
                    }
                }
            }
        }
    }

    public void somaMaxId(int id) {
        using (FileStream file = new FileStream(contaDb, FileMode.Open)) {
            using (BinaryWriter writer = new BinaryWriter(file)) {
                writer.Seek(0, SeekOrigin.Begin);
                writer.Write(id);
            }
        }
    }

    public bool DeleteById(int id){
        long position = FindByIndex(id);
        using (FileStream file = new FileStream(contaDB, FileMode.Open, FileAccess.Read)){
            using (BinaryReader reader = new BinaryReader(file)){
                reader.BaseStream.Position = position;
                conta.Lapide = reader.ReadBoolean();
                reader.ReadInt32();
                conta.Deserialize(reader);
                
                if (conta.id == contaId) 
                    return conta;
                 else 
                    return null;
                
            }
        }
    }

    public ContaBancaria? ReadById(int id) {
        ContaBancaria conta = new ContaBancaria();
        long pos = FindPosByIndex(id);
        using (FileStream file = new FileStream(contaDB, FileMode.Open, FileAccess.Read)) {
            using (BinaryReader reader = new BinaryReader(file)) {
                reader.BaseStream.Position = pos;
                conta.Lapide = reader.ReadBoolean();
                reader.ReadInt32();
                conta.Deserialize(reader);
                
                if (conta.id == id) 
                    return conta;
                else 
                    return null;
                
            }
        }
    }

    public List<ContaBancaria> ReadAll() {
        List<ContaBancaria> list = new List<ContaBancaria>();

        using (FileStream file = new FileStream(indexDb, FileMode.Open, FileAccess.Read)) {
            using (BinaryReader reader = new BinaryReader(file)) {
                while (reader.PeekChar() != -1) {
                    ContaBancaria? conta = new ContaBancaria();

                    reader.ReadUInt16();
                    conta = ReadByPos(reader.ReadInt64());	
                    if (conta != null && !conta.Lapide) {
                        list.Add(conta);
                    }
                }
            }
        }

        return list;
    }

    public List<int> ReadAll(string str) {
        List<int> list = new List<int>();

        using (FileStream file = new FileStream(indexDb, FileMode.Open, FileAccess.Read)) {
            using (BinaryReader reader = new BinaryReader(file)) {
                while (reader.PeekChar() != -1) {
                    ContaBancaria? conta = new ContaBancaria();

                    reader.ReadUInt16();
                    conta = ReadByPos(reader.ReadInt64());
                    if (conta != null && !conta.Lapide) {
                        if (str.Length == 2 && str == conta.Cidade) {
                            list.Add(conta.id);
                        }
                        if (str == conta.NomePessoa) {
                            list.Add(conta.id);
                        }
                    }
                }
            }
        }
        return list;
    }

    public ContaBancaria? ReadByPos(long pos) {
        ContaBancaria conta = new ContaBancaria();
        using (FileStream file = new FileStream(contaDb, FileMode.Open, FileAccess.Read)) {
            using (BinaryReader reader = new BinaryReader(file)) {
                reader.BaseStream.Position = pos;
                conta.Lapide = reader.ReadBoolean();
                reader.ReadInt32();
                conta.Deserialize(reader);
                return conta;
            }
        }
    }

    public List<ContaBancaria> ReadByCity(string city) {
        List<int> ids = new List<int>();
        List<ContaBancaria> contas = new List<ContaBancaria>();

        using (FileStream file = new FileStream(invertedCity, FileMode.Open, FileAccess.Read)) {
            using (BinaryReader reader = new BinaryReader(file)) {
                reader.BaseStream.Seek(0, SeekOrigin.Begin);
                while (reader.PeekChar() != -1) {
                    string cidade = reader.ReadString();
                    int length = reader.ReadInt32();

                    if (city == cidade) {
                        for (int i = 0; i < length; i++) {
                            ids.Add(reader.ReadUInt16());
                        }
                    } else {
                        reader.BaseStream.Position += length * 2;
                    }
                }
            }
        }

        for (int i = 0; i < ids.Count; i++) {
            contas.Add(ReadByPos(FindPosByIndex(ids.ElementAt(i))));
        }

        return contas;
    }

    public List<ContaBancaria> ReadByName(string name) {
        List<int> ids = new List<int>();
        List<ContaBancaria> contas = new List<ContaBancaria>();

        using (FileStream file = new FileStream(invertedName, FileMode.Open, FileAccess.Read)) {
            using (BinaryReader reader = new BinaryReader(file)) {
                while (reader.PeekChar() != -1) {
                    string nome = reader.ReadString();
                    int length = reader.ReadInt32();
                    if (name.CompareTo(nome) == 0) {
                        for (int i = 0; i < length; i++) {
                            ids.Add(reader.ReadUInt16());
                        }
                    } else {
                        reader.BaseStream.Position += length * 2;
                    }
                }
            }
        }

        for (int i = 0; i < ids.Count; i++) {
            contas.Add(ReadByPos(FindPosByIndex(ids.ElementAt(i))));
        }

        return contas;
    }

    public void UpdateById(ContaBancaria conta, int id) {

        conta.id = id;
        byte[] buffer = conta.Serialize();
        long position = FindPosByIndex(id);

        using (FileStream file = new FileStream(contaDb, FileMode.Open, FileAccess.ReadWrite)) {
            using (BinaryReader reader = new BinaryReader(file)) {
                reader.BaseStream.Position = position;
                bool lapide = reader.ReadBoolean();
                int tam = reader.ReadInt32();

                if (lapide) {
                    Create(conta);
                } else {
                    if (tam < buffer.Length) {
                        DeleteById(id, file);
                        Create(conta);
                    } else {
                        using (BinaryWriter writer = new BinaryWriter(file)) {
                            writer.Write(buffer);
                        }
                    }
                }
            }
        }
        CreateInvertedFiles();
    }

    private void BuildIndexFile() {
        if (!File.Exists(pathIndexDB)) {
            File.Create(pathIndexDB);
        }
    }

    private void BuildInvertedFiles() {
        if (!File.Exists(invertedName)) {
            File.Create(invertedName);
        }
        if (!File.Exists(invertedCity)) {
            File.Create(invertedCity);
        }
    }

    public void CreateIndex(int id, long pos) {
        using (FileStream stream = new FileStream(pathIndexDB, FileMode.Open, FileAccess.Write)) {
            using (BinaryWriter writer = new BinaryWriter(stream)) {
                writer.Seek(0, SeekOrigin.End);
                writer.Write(id);
                writer.Write(pos);
            }
        }
    }

    public long FindPosByIndex(int index) {
        using (FileStream stream = new FileStream(pathIndexDB, FileMode.Open, FileAccess.Read)) {
            using (BinaryReader reader = new BinaryReader(stream)) {
                long len = reader.BaseStream.Length, inf = 0, sup = len, meio;
                reader.BaseStream.Seek(0, SeekOrigin.Begin);
                while (reader.PeekChar() != -1 && inf <= sup) {
                    meio = (inf + sup) / 2;

                    if (meio % 10 <= 5) {
                        meio -= meio % 10;
                    } else {
                        meio += meio % 10;
                    }

                    reader.BaseStream.Position = meio;

                    int id = reader.ReadUInt16();
                    if (id == index) {
                        return reader.ReadInt64();
                    } else if (id > index) {
                        sup = meio;
                    } else {
                        inf = meio;
                    }
                }
            }
        }
        return -1;
    }

    public void CreateInvertedFiles() {
        List<ContaBancaria> list = ReadAll();
        List<string> cities = new List<string>();
        List<string> names = new List<string>();
        List<int> ids = new List<int>();

        // Listas de cidades e nomes sem repetição
        for (int i = 0; i < list.Count; i++) {
            if (!cities.Contains(list[i].Cidade)) {
                cities.Add(list[i].Cidade);
            }
            if (!names.Contains(list[i].NomePessoa)) {
                names.Add(list[i].NomePessoa);
            }
        }

        cities.Sort();
        names.Sort();


        using (FileStream stream = new FileStream(invertedCity, FileMode.Open, FileAccess.Write)) {
            using (BinaryWriter writer = new BinaryWriter(stream)) {
                for (int i = 0; i < cities.Count; i++) {
                    ids = ReadAll(cities[i]);
                    writer.Write(cities[i]);
                    writer.Write(ids.Count);
                    for (int j = 0; j < ids.Count; j++) {
                        writer.Write(ids[j]);
                    }
                }
            }
        }
        using (FileStream stream = new FileStream(invertedName, FileMode.Open, FileAccess.Write)) {
            using (BinaryWriter writer = new BinaryWriter(stream)) {
                for (int i = 0; i < names.Count; i++) {
                    ids = ReadAll(names[i]);
                    writer.Write(names[i]);
                    writer.Write(ids.Count);
                    for (int j = 0; j < ids.Count; j++) {
                        writer.Write(ids[j]);
                    }
                }
            }
        }
    }
}