    public class BaseFixture : IDisposable
    {
        private MongoDbRunner _mongoRunner;
        private MongoClient _mongoClient;
        private SdwanMongoContext _sDWANMongoContext;
        private string DatabaseName = "sdwan";
        private string _dataFilePath = "";

        public IMapper _mapper;

        private IInterfaceStatsRepository _intRepo;
        private ILinkQualityStatsRepository _linkQualityRepo;
        private ITunnelStatsRepository _tunnelRepo;
        private IAppCategoryStatsRepository _appCatRepo;
        private IAppStatsRepository _appRepo;
        private ISourceStatsRepository _sourceRepo;
        private IDestinationStatsRepository _destinationRepo;
        private IDeviceStatsRepository _devStatsRepo;
        private IShapingPolicyStatsRepository _shapingPolicyStatsRepo;
        private IThreatStatsRepository _threatStatsRepo;
        private IWebsiteStatsRepository _websiteStatsRepo;

        public BaseFixture()
        {
            _mongoRunner = MongoDbRunner.Start();
            _mongoClient = new MongoClient(_mongoRunner.ConnectionString);
            _sDWANMongoContext = new SdwanMongoContext(_mongoClient, DatabaseName);
            _dataFilePath = $"{Directory.GetCurrentDirectory().Split("bin\\")[0]}//Data//";

            var mappingConfig = new MapperConfiguration(cfg =>
            {
                cfg.AllowNullCollections = true;
                cfg.AddProfile<MappingProfile>();
            });
            _mapper = mappingConfig.CreateMapper();

            //map domain models to mongo infra
            var runningAssemblies = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.GetName().Name.Contains("Singtel.PE.SDWAN.Infrastructure"));
            foreach (var assembly in runningAssemblies)
            {
                var types = assembly.GetExportedTypes()
                .Where(x => x.GetInterfaces().Any(y => y.IsGenericType && y.GetGenericTypeDefinition() == typeof(IMapDomainToInfra<>)))
                .ToList();

                foreach (var type in types)
                {
                    var instance = Activator.CreateInstance(type);

                    var methodInfo = type.GetMethod("Map") ?? type.GetInterface("IMapDomainToInfra`1").GetMethod("Map");

                    methodInfo?.Invoke(instance, null);
                }
            }

            //seed db
            SeedCollections();
        }

        /// <summary>
        /// Returns InfraAssetsClientFixture with Device as entity 
        /// </summary>
        /// <returns></returns>
        public Mock<IRefitClientFactory> MockIInfraAssetsDeviceClientFactory()
        {
            var mockClient = new Mock<IRefitClientFactory>();
            mockClient.Setup(client => client.CreateClient<IInfraAssetsClient<InfrastructureAssetResponse>>(It.IsAny<string>(), It.IsAny<InternalMsApiUrl>())).ReturnsAsync(SeedInfraAssetsHelper.MockInfraAssetsDeviceRestClient().Object);
            return mockClient;
        }

        /// <summary>
        /// Returns InfraAssetsClientFixture with dynamic entity object.
        /// </summary>
        /// <returns></returns>
        public Mock<IRefitClientFactory> MockIInfraAssetsDynamicClientFactory()
        {
            var mockClient = new Mock<IRefitClientFactory>();
            mockClient.Setup(client => client.CreateClient<IInfraAssetsClient<dynamic>>(It.IsAny<string>(), It.IsAny<InternalMsApiUrl>())).ReturnsAsync(SeedInfraAssetsHelper.MockInfraAssetsDynamicRestClient().Object);
            return mockClient;
        }

        public Mock<IRefitClientFactory> MockCustInventoryClientFactory()
        {
            var mockClient = new Mock<IRefitClientFactory>();
            mockClient.Setup(client => client.CreateClient<ICustomerInventoryClient>(It.IsAny<string>(), It.IsAny<InternalMsApiUrl>())).ReturnsAsync(SeedCustomerInventoryHelper.MockICustomerInventoryClient().Object);

            mockClient.Setup(client => client.CreateClient<IIncidentsApi>(It.IsAny<string>(), It.IsAny<InternalMsApiUrl>())).ReturnsAsync(SeedIncidentManagementHelper.MockIncidentManagement().Object);
            return mockClient;
        }

        public Mock<ISdwanServiceFactory> MockSdwanServiceFactory()
        {
            var mockClient = new Mock<ISdwanServiceFactory>();
            mockClient.Setup(client => client.CreateService(SdwanBrand.VeloCloud.Name)).Returns(SeedVeloCloudServiceHelper.MockVeloCloudService().Object);
            mockClient.Setup(client => client.CreateService(SdwanBrand.Fortinet.Name)).Returns(SeedFortinetServiceHelper.MockFortinetService().Object);
            return mockClient;
        }

        public void Dispose()
        {
            _mongoRunner.Dispose();
            _mongoClient = null;
        }

        #region "Repo"
        public ILinkQualityStatsRepository GetLinkQualityStatisticRepository()
        {
            return _linkQualityRepo;
        }
        public IInterfaceStatsRepository GetInterfaceStatisticRepository()
        {
            return _intRepo;
        }
        public ITunnelStatsRepository GetTunnelStatisticRepository()
        {
            return _tunnelRepo;
        }
        public IAppCategoryStatsRepository GetAppCategoryStatsRepository()
        {
            return _appCatRepo;
        }

        public IAppStatsRepository GetAppStatsRepository()
        {
            return _appRepo;
        }
        public IDeviceStatsRepository GetDeviceStatsRepository()
        {
            return _devStatsRepo;
        }
        public ISourceStatsRepository GetSourceStatsRepository()
        {
            return _sourceRepo;
        }
        public IDestinationStatsRepository GetDestinationStatsRepository()
        {
            return _destinationRepo;
        }

        public IShapingPolicyStatsRepository GetShapingPolicyStatsRepository()
        {
            return _shapingPolicyStatsRepo;
        }

        public IThreatStatsRepository GetThreatStatsRepository()
        {
            return _threatStatsRepo;
        }

        public IWebsiteStatsRepository GetWebsiteStatsRepository()
        {
            return _websiteStatsRepo;
        }

        #endregion

        #region "Seed"
        private void SeedCollections()
        {
            SeedCollectionHelper.SeedTestData<InterfaceStatistic>(_sDWANMongoContext.InterfaceStatistics, _dataFilePath, "sdwan.interfaceStats");
            this._intRepo = new InterfaceStatsRepository(_sDWANMongoContext);

            SeedCollectionHelper.SeedTestData<TunnelStatistic>(_sDWANMongoContext.TunnelStatistics, _dataFilePath, "sdwan.tunnelStats");
            this._tunnelRepo = new TunnelStatsRepository(_sDWANMongoContext);

            SeedCollectionHelper.SeedTestData<AppCategoryStatistic>(_sDWANMongoContext.AppCategoryStatistics, _dataFilePath, "sdwan.appCategoryStats");
            this._appCatRepo = new AppCategoryStatsRepository(_sDWANMongoContext);

            SeedCollectionHelper.SeedTestData<AppStatistic>(_sDWANMongoContext.AppStatistics, _dataFilePath, "sdwan.appStats");
            this._appRepo = new AppStatsRepository(_sDWANMongoContext);

            SeedCollectionHelper.SeedTestData<SourceStatistic>(_sDWANMongoContext.SourceStatistics, _dataFilePath, "sdwan.sourceStats");
            this._sourceRepo = new SourceStatsRepository(_sDWANMongoContext);

            SeedCollectionHelper.SeedTestData<DeviceStatistic>(_sDWANMongoContext.DeviceStatistics, _dataFilePath, "sdwan.deviceStats");
            this._devStatsRepo = new DeviceStatsRepository(_sDWANMongoContext);

            SeedCollectionHelper.SeedTestData<DestinationStatistic>(_sDWANMongoContext.DestinationStatistics, _dataFilePath, "sdwan.destinationStats");
            this._destinationRepo = new DestinationStatsRepository(_sDWANMongoContext);

            SeedCollectionHelper.SeedTestData<LinkQualityStatistic>(_sDWANMongoContext.LinkQualityStatistic, _dataFilePath, "sdwan.linkQualityStats");
            this._linkQualityRepo = new LinkQualityStatsRepository(_sDWANMongoContext);

            SeedCollectionHelper.SeedTestData<ShapingPolicyStatistic>(_sDWANMongoContext.ShapingPolicyStatistics, _dataFilePath, "sdwan.shapingPolicyStats");
            this._shapingPolicyStatsRepo = new ShapingPolicyStatsRepository(_sDWANMongoContext);

            SeedCollectionHelper.SeedTestData<ThreatStatistic>(_sDWANMongoContext.ThreatStatistics, _dataFilePath, "sdwan.threatStats");
            this._threatStatsRepo = new ThreatStatsRepository(_sDWANMongoContext);

            SeedCollectionHelper.SeedTestData<WebsiteStatistic>(_sDWANMongoContext.WebsiteStatistics, _dataFilePath, "sdwan.websiteStats");
            this._websiteStatsRepo = new WebsiteStatsRepository(_sDWANMongoContext);

        }
        #endregion
    }
