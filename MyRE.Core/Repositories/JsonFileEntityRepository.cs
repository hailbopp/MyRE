using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Optional;

namespace MyRE.Core.Repositories
{
    public class JsonFileEntityRepository<TEntity, TId> : IEntityRepository<TEntity, TId>
    {
        private const string Extension = ".entity.json";
        private readonly int ExtensionLength = Extension.Length;

        private readonly string _directoryPath;
        private readonly Func<TEntity, TId> _getId;
        private readonly Action<TEntity, TId> _setId;
        private readonly Func<IEnumerable<TId>, TId> _createNewId;
        private readonly Func<string, TId> _parseId;

        protected JsonFileEntityRepository(string directory, Func<TEntity, TId> idSelector, Action<TEntity, TId> setId, Func<IEnumerable<TId>, TId> createNewId, Func<string, TId> parseId)
        {
            _directoryPath = directory;
            _getId = idSelector;
            _createNewId = createNewId;
            _parseId = parseId;
            _setId = setId;

            if (!Directory.Exists(_directoryPath))
            {
                Directory.CreateDirectory(_directoryPath);
            }
        }

        protected virtual string EntityToJson(TEntity entity) => JsonConvert.SerializeObject(entity, Formatting.Indented);
        protected virtual TEntity JsonToEntity(string json) => JsonConvert.DeserializeObject<TEntity>(json);

        protected async Task<string> ReadFileAsync(string path)
        {
            using (var reader = File.OpenText(path))
            {
                return await reader.ReadToEndAsync();
            }
        }

        protected async Task WriteFileAsync(string path, string data)
        {
            var dataBytes = Encoding.Default.GetBytes(data);

            using (var writer = File.OpenWrite(path))
            {
                await writer.WriteAsync(dataBytes, 0, dataBytes.Length);
            }
        } 

        protected static bool IsNullOrDefault<T>(T argument)
        {
            // deal with normal scenarios
            if (argument == null) return true;
            if (object.Equals(argument, default(T))) return true;

            // deal with non-null nullables
            Type methodType = typeof(T);
            if (Nullable.GetUnderlyingType(methodType) != null) return false;

            // deal with boxed value types
            Type argumentType = argument.GetType();
            if (argumentType.IsValueType && argumentType != methodType)
            {
                object obj = Activator.CreateInstance(argument.GetType());
                return obj.Equals(argument);
            }

            return false;
        }

        protected string GetFilePathForId(TId entityId) => Path.Combine(_directoryPath, entityId.ToString() + Extension);

        public async Task<Option<TEntity>> AddAsync(TEntity entity)
        {
            var entityId = _getId(entity);

            if (IsNullOrDefault(entityId))
            {
                _setId(entity, _createNewId(await GetKeysAsync()));
            }
            else if (File.Exists(GetFilePathForId(entityId)))
            {
                throw new ConstraintException($"Duplicate primary key {entityId} in store `{_directoryPath}`");
            }
            else
            {
                // ID is explicitly set, and doesn't already exist. So no problem here.
            }

            await WriteFileAsync(GetFilePathForId(_getId(entity)), EntityToJson(entity));

            return await GetByIdAsync(_getId(entity));
        }

        public async Task DeleteAsync(TId id)
        {
            File.Delete(GetFilePathForId(id));
        }

        private IEnumerable<string> GetFilePaths()
        {
            return Directory.EnumerateFiles(_directoryPath)
                .Where(f => f.EndsWith(Extension));
        }

        public async Task<IEnumerable<TId>> GetKeysAsync()
        {
            return GetFilePaths().Select(f => f.Substring(0, f.Length - ExtensionLength)).Select(_parseId);
        }

        public async Task<Option<TEntity>> GetByIdAsync(TId id)
        {
            var filename = Path.Combine(_directoryPath, $"{id.ToString()}{Extension}");
            if (File.Exists(filename))
            {
                var fileContents = await ReadFileAsync(filename);
                return Option.Some(JsonToEntity(fileContents));
            }
            else
            {
                return Option.None<TEntity>();
            }
        }

        public async Task<IEnumerable<TEntity>> EnumerateAsync()
        {
            return GetFilePaths().Select(fp => ReadFileAsync(fp).Result).Select(JsonToEntity);
        }


    }
}